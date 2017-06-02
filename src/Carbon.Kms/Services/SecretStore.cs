using System;
using System.Threading.Tasks;

using Carbon.Data.Expressions;

using Dapper;

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
            // TODO: Build a custom sql segment...

            using (var context = db.Context.GetConnection())
            {
                await context.ExecuteAsync(
                    @"UPDATE `Secrets`
                      SET `deleted` = NOW()
                      WHERE `id` = @id", secret
                );
            }
        }

        private async Task MarkAccessed(SecretInfo secret)
        {
            using (var context = db.Context.GetConnection())
            {
                await context.ExecuteAsync(
                    @"UPDATE `Secrets`
                      SET `accessed` = NOW()
                      WHERE `id` = @id", secret
                );
            }
        }
    }
}
