using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Data.Protection;
using Carbon.Data.Sequences;

namespace Carbon.Kms.Services
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
                ?? throw new KeyNotFoundException(id.ToString());
        }

        public async Task<KeyInfo> GetAsync(long ownerId, string name)
        {
            Ensure.IsValidId(ownerId,   nameof(ownerId));
            Ensure.NotNullOrEmpty(name, nameof(name));

            var keys = await db.Keys.QueryAsync(Conjunction(
                Eq("ownerId", ownerId),
                Eq("name", name),
                IsNotNull("activated"),
                Lte("activated", Now),      // activated
                IsNull("deleted")                   // not deleted
            ), Order.Descending("expires"), take: 1);

            if (keys.Count == 0)
            {
                throw new Exception($"no active keys for '{ownerId}/{name}'");
            }

            return keys[0];
        }

        public Task<IReadOnlyList<KeyInfo>> ListAsync(long ownerId)
        {
            Ensure.IsValidId(ownerId, nameof(ownerId));

            return db.Keys.QueryAsync(And(Eq("ownerId", ownerId), IsNull("deleted")));
        }

        public Task<IReadOnlyList<KeyInfo>> ListAsync(long ownerId, string name)
        {
            Ensure.IsValidId(ownerId,   nameof(ownerId));
            Ensure.NotNullOrEmpty(name, nameof(name));

            return db.Keys.QueryAsync(Conjunction(
                Eq("ownerId", ownerId), 
                Eq("name", name),
                IsNull("deleted")
            ));
        }

        public async Task CreateAsync(KeyInfo key)
        {
            Ensure.NotNull(key, nameof(key));

            await db.Keys.InsertAsync(key);
        }

        public async Task DeactivateAsync(Uid id)
        {
            await db.Keys.PatchAsync(id, changes: new[] {
                Change.Remove("activated"),
                Change.Replace("status", KeyStatus.Deactivated)
            });
        }

        public async Task DeleteAsync(Uid id)
        {
            await db.Keys.PatchAsync(id, changes: new[] {
                Change.Remove("data"),
                Change.Replace("deleted", Now)
            }, condition: IsNull("deleted"));
        }
    }
}
