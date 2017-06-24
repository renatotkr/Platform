using System;
using System.Threading.Tasks;

using Carbon.Data.Protection;
using Carbon.Platform.Resources;
using Carbon.Time;

namespace Carbon.Kms
{
    public class KeyProvider : IKeyProvider
    {
        private readonly KmsDb db;
        private readonly IClock clock;
        private readonly IKeyProtectorProvider vaultProvider;

        public KeyProvider(
            IClock clock,
            IKeyProtectorProvider vaultProvider,
            KmsDb db)
        {
            this.db            = db            ?? throw new ArgumentNullException(nameof(db));
            this.clock         = clock         ?? throw new ArgumentNullException(nameof(clock));
            this.vaultProvider = vaultProvider ?? throw new ArgumentNullException(nameof(vaultProvider));
        }   

        public async ValueTask<CryptoKey> GetAsync(long id)
        {
            var key = await db.Keys.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.VaultKey, id);

            var result = await DecryptAsync(key).ConfigureAwait(false);

            return new CryptoKey(
                id    : key.Id.ToString(),
                value : result
            );
        }

        private async Task<byte[]> DecryptAsync(KeyInfo key)
        {
            #region Preconditions

            if (key == null)
                throw new ArgumentNullException(nameof(key));
         
            if (key.Expires != null && key.Expires < clock.Observe())
                throw new Exception($"key#{key.Id} is expired");

            #endregion

            var kek = await vaultProvider.GetAsync(key.KekId).ConfigureAwait(false);

            return await kek.DecryptAsync(
                key.Ciphertext,
                key.GetAuthenticatedData()
            ).ConfigureAwait(false);
        }
    }
}
