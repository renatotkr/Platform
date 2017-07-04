using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Data.Protection;
using Carbon.Data.Sequences;

namespace Carbon.Kms
{
    using static Expression;

    public class KeyStore : IKeyStore
    {
        private readonly KmsDb db;

        public KeyStore(KmsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<KeyInfo> GetAsync(Uid id)
        {
            return await db.Keys.FindAsync(id)
                ?? throw new Exception($"key#{id} not found");
        }

        public async Task<KeyInfo> GetAsync(long ownerId, string name)
        {
            var keys = await db.Keys.QueryAsync(Conjunction(
                Eq("ownerId", ownerId),
                Eq("name", name),
                IsNull("deleted")
            ), Order.Descending("expires"), take: 1);

            return keys.FirstOrDefault();
        }

        public Task<IReadOnlyList<KeyInfo>> ListAsync(long ownerId)
        {
            return db.Keys.QueryAsync(And(Eq("ownerId", ownerId), IsNull("deleted")));
        }

        public Task<IReadOnlyList<KeyInfo>> ListAsync(long ownerId, string name)
        {
            return db.Keys.QueryAsync(Conjunction(
                Eq("ownerId", ownerId), 
                Eq("name", name),
                IsNull("deleted")
            ));
        }

        public async Task CreateAsync(KeyInfo key)
        {
            #region Preconditions

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            #endregion

            await db.Keys.InsertAsync(key).ConfigureAwait(false);
        }

        public async Task DeactivateAsync(Uid id)
        {
            await db.Keys.PatchAsync(id, changes: new[] {
                Change.Remove("activated"),
                Change.Replace("status", KeyStatus.Deactivated)
            }).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Uid id)
        {
            await db.Keys.PatchAsync(id, changes: new[] {
                Change.Remove("data"),
                Change.Replace("deleted", Func("NOW"))
            }).ConfigureAwait(false);
        }
    }
}
