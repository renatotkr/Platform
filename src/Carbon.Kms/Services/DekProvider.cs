using System;
using System.Linq;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Data.Protection;
using Carbon.Time;

namespace Carbon.Kms
{
    using static Expression;

    public class DekProvider : IDekProvider
    {
        private readonly KmsDb db;
        private readonly IClock clock;
        private readonly IDataProtectorProvider masterKeyProvider;

        public DekProvider(
            IClock clock,
            IDataProtectorProvider masterKeyProvider,
            KmsDb db)
        {
            this.db                = db                ?? throw new ArgumentNullException(nameof(db));
            this.clock             = clock             ?? throw new ArgumentNullException(nameof(clock));
            this.masterKeyProvider = masterKeyProvider ?? throw new ArgumentNullException(nameof(masterKeyProvider));
        }

        public async ValueTask<DataKey> GetAsync(long id)
        {
            var key = (await db.Deks.QueryAsync(
                expression : And(Eq("id", id), IsNull("deleted")),
                order      : Order.Descending("version"),
                take       : 1
            ).ConfigureAwait(false)).FirstOrDefault();

            if (key == null)
            {
                throw new Exception($"dek#{id} does not exist");
            }

            var result = await DecryptAsync(key);

            return new DataKey(key.Id, key.Version, result);
        }

        public async ValueTask<DataKey> GetAsync(long id, int version)
        {
            var key = await db.Deks.FindAsync((id, version))
                ?? throw new Exception($"dek#{id}@{version} not found");

            var result = await DecryptAsync(key).ConfigureAwait(false);

            return new DataKey(key.Id, key.Version, result);
        }

        private async Task<byte[]> DecryptAsync(DekInfo dek)
        {
            #region Preconditions

            if (dek == null)
                throw new ArgumentNullException(nameof(dek));
         
            if (dek.Expires != null && dek.Expires < clock.Observe())
                throw new Exception($"dek#{dek.Id}@{dek.Version} is expired and may not be decrypted");

            #endregion

            var kek = await masterKeyProvider.GetAsync(dek.KekId).ConfigureAwait(false);

            return await kek.DecryptAsync(
                dek.Ciphertext,
                dek.GetAuthenticatedData()
            ).ConfigureAwait(false);
        }
    }
}
