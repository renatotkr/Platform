using System;
using System.Linq;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Data.Protection;
using Carbon.Platform.Resources;
using Carbon.Time;

namespace Carbon.Kms
{
    using static Expression;

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

        public async ValueTask<DataKey> GetAsync(long keyId)
        {
            var key = (await db.Keys.QueryAsync(
                expression : And(Eq("id", keyId), IsNull("deleted")),
                order      : Order.Descending("version"),
                take       : 1
            ).ConfigureAwait(false)).FirstOrDefault();

            if (key == null)
            {
                throw ResourceError.NotFound(ResourceTypes.VaultKey, keyId);
            }

            var result = await DecryptAsync(key);

            return new DataKey(key.Id.ToString(), key.Version, result);
        }

        public async ValueTask<DataKey> GetAsync(long keyId, int keyVersion)
        {
            var key = await db.Keys.FindAsync((keyId, keyVersion))
                ?? throw new Exception($"key#{keyId}@{keyVersion} not found");

            var result = await DecryptAsync(key).ConfigureAwait(false);

            return new DataKey(
                id      : key.Id.ToString(),
                version : key.Version,
                value   : result
            );
        }

        private async Task<byte[]> DecryptAsync(KeyInfo key)
        {
            #region Preconditions

            if (key == null)
                throw new ArgumentNullException(nameof(key));
         
            if (key.Expires != null && key.Expires < clock.Observe())
                throw new Exception($"key#{key.Id}@{key.Version} is expired and may not be decrypted");

            #endregion

            var kek = await vaultProvider.GetAsync(key.KekId).ConfigureAwait(false);

            return await kek.DecryptAsync(
                key.Ciphertext,
                key.GetAuthenticatedData()
            ).ConfigureAwait(false);
        }
    }
}
