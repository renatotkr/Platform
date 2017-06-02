using System;
using System.Threading.Tasks;

using Carbon.Data.Protection;
using Carbon.Time;

namespace Carbon.Kms
{
    public class SecretManager : ISecretManager
    {
        private readonly IClock clock;
        private readonly ISecretStore secretStore;
        private readonly IDekProvider dekProvider;
        private readonly long ownerId;

        public SecretManager(
            IClock clock,
            IDekProvider dekProvider,
            ISecretStore secretStore, 
            long ownerId)
        {
            this.clock       = clock       ?? throw new ArgumentNullException(nameof(clock));
            this.dekProvider = dekProvider ?? throw new ArgumentNullException(nameof(dekProvider));
            this.secretStore = secretStore ?? throw new ArgumentNullException(nameof(secretStore));
            this.ownerId     = ownerId;
        }

        public async Task<byte[]> DecryptAsync(string name)
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
            
            var dek = await dekProvider.GetAsync(secret.KeyId, secret.KeyVersion).ConfigureAwait(false);

            using (var protector = new AesDataProtector(dek.Value, secret.IV))
            {
                return protector.Decrypt(secret.Ciphertext);
            }
        }

        public async Task CreateAsync(string name, byte[] value, long keyId, DateTime? expires)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            #endregion

            // TODO....

            var dek = await dekProvider.GetAsync(keyId);

            // should every secret have it's own DEK?
            var iv = Secret.Generate(16).Value;
            byte[] ciphertext;

            using (var protector = new AesDataProtector(dek.Value, iv))
            {
                ciphertext = protector.Encrypt(value);
            }

            // id is assigned on insert...
            var secret = new SecretInfo(
                name       : name,
                keyId      : dek.Id,
                keyVersion : dek.Version,
                iv         : iv,
                ciphertext : ciphertext,
                expires    : expires,
                ownerId    : ownerId
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