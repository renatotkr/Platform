using System;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;

namespace Carbon.Kms
{
    using static Expression;

    public class SecretStore : ISecretStore
    {
        private readonly KmsDb db;

        public SecretStore(KmsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task AddAsync(SecretInfo secret)
        {
            await db.Secrets.InsertAsync(secret);
        }

        public async Task<SecretInfo> FindAsync(long ownerId, string name)
        {
            var secret = await db.Secrets.QueryFirstOrDefaultAsync(
                Conjunction(
                    Eq("ownerId", ownerId), 
                    Eq("name", name), 
                    IsNull("deleted"))
            ).ConfigureAwait(false);

            if (secret != null)
            {
                await MarkAccessed(secret);
            }

            return secret;
        }

        public async Task<SecretInfo> GetAsync(long id)
        {
            var secret = await db.Secrets.FindAsync(id).ConfigureAwait(false)
                ?? throw new Exception($"secret#{id} not found");

            await MarkAccessed(secret);

            return secret;
        }

        public async Task RemoveAsync(SecretInfo secret)
        {
            await db.Secrets.PatchAsync(
               key      : secret.Id,
               changes : new[] { Change.Replace("deleted", Func("NOW")) }
            ).ConfigureAwait(false);
        }

        private async Task MarkAccessed(SecretInfo secret)
        {
            await db.Secrets.PatchAsync(
                 key     : secret.Id,
                 changes : new[] { Change.Replace("accessed", Func("NOW")) }
            ).ConfigureAwait(false);
        }
    }
}
