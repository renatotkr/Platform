using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Data.Sequences;

namespace Carbon.Kms.Services
{
    public sealed class KeyGrantService : IKeyGrantService
    {
        private readonly KmsDb db;

        public KeyGrantService(KmsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<KeyGrant> GetAsync(Uid id)
        {
            return await db.KeyGrants.FindAsync(id);

            // TODO: throw if not found
        }

        public async Task<IReadOnlyList<KeyGrant>> ListAsync(Uid keyId)
        {
            return await db.KeyGrants.QueryAsync(Expression.Eq("keyId", keyId));
        }

        public async Task<KeyGrant> CreateAsync(CreateKeyGrantRequest request)
        {
            Validate.NotNull(request, nameof(request));

            var grant = new KeyGrant(
                grantId     : Guid.NewGuid(),
                keyId       : request.KeyId,
                name        : request.Name,
                actions     : request.Actions,
                userId      : request.UserId,
                constraints : request.Constraints,
                externalId  : request.ExternalId,
                properties  : request.Properties
            );

            await db.KeyGrants.InsertAsync(grant);

            return grant;
        }

        public async Task<bool> DeleteAsync(KeyGrant grant)
        {
            Validate.NotNull(grant, nameof(grant));

            return await db.KeyGrants.PatchAsync(grant.Id, new[] {
                Change.Replace("deleted", Expression.Func("NOW"))
            }, Expression.IsNull("deleted")) > 0;
        }
    }
}
