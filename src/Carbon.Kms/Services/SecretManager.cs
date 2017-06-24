using System;
using System.Threading.Tasks;

using Carbon.Data.Protection;
using Carbon.Time;

namespace Carbon.Kms
{
    public class SecretManager : ISecretManager
    {
        private readonly long vaultId;
        private readonly IClock clock;
        private readonly ISecretStore secretStore;
        private readonly IKeyProvider keyProvider;
        private readonly long ownerId;

        public SecretManager(
            long vaultId,
            IClock clock,
            IKeyProvider keyProvider,
            ISecretStore secretStore, 
            long ownerId)
        {
            this.vaultId     = vaultId;
            this.clock       = clock       ?? throw new ArgumentNullException(nameof(clock));
            this.keyProvider = keyProvider ?? throw new ArgumentNullException(nameof(keyProvider));
            this.secretStore = secretStore ?? throw new ArgumentNullException(nameof(secretStore));
            this.ownerId     = ownerId;
        }

        public async Task<byte[]> GetAsync(string name)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            var secret = await secretStore.FindAsync(ownerId, name).ConfigureAwait(false);

            if (secret.Expires != null && secret.Expires <= clock.Observe())
            {
                throw new Exception("Secret is expired");
            }
            
            var key = await keyProvider.GetAsync(secret.KeyId).ConfigureAwait(false);

            using (var protector = new AesDataProtector(key.Value, secret.IV))
            {
                return protector.Decrypt(secret.Ciphertext);
            }
        }

        public async Task CreateAsync(
            string name,
            byte[] value,
            long keyId, 
            DateTime? expires)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            #endregion

            var key = await keyProvider.GetAsync(keyId);

            var iv = Secret.Generate(16).Value;
            byte[] ciphertext;

            using (var protector = new AesDataProtector(key.Value, iv))
            {
                ciphertext = protector.Encrypt(value);
            }
            
            var secret = new SecretInfo(
                id         : 0, // assigned on insert
                name       : name,
                keyId      : keyId,
                iv         : iv,
                ciphertext : ciphertext,
                expires    : expires,
                ownerId    : ownerId,
                vaultId    : vaultId
            );

            await secretStore.AddAsync(secret).ConfigureAwait(false);
        }

        public async Task DeleteAsync(long id)
        {
            var secret = await secretStore.GetAsync(id).ConfigureAwait(false);

            await secretStore.RemoveAsync(secret);
        }
    }
}