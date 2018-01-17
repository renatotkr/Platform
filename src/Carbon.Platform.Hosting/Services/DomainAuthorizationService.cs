using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Hosting
{
    public class DomainAuthorizationService : IDomainAuthorizationService
    {
        private readonly HostingDb db;

        public DomainAuthorizationService(HostingDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<DomainAuthorization>> ListAsync(IDomain domain)
        {
            Ensure.NotNull(domain, nameof(domain));

            var range = ScopedId.GetRange(domain.Id);

            return db.DomainAuthorizations.QueryAsync(
                Expression.Between("id", range.Start, range.End)
            );
        }
        
        public async Task<DomainAuthorization> GetLatestAsync(IDomain domain)
        {
            Ensure.NotNull(domain, nameof(domain));

            var range = ScopedId.GetRange(domain.Id);

            return (await db.DomainAuthorizations.QueryAsync(
               Expression.Between("id", range.Start, range.End),
               order: Order.Descending("id"),
               take: 1
           )).FirstOrDefault();
        }

        public async Task<DomainAuthorization> CreateAsync(CreateDomainAuthorizationRequest request)
        {
            Ensure.NotNull(request, nameof(request));

            var id = await DomainAuthorizationId.NextAsync(db.Context, request.DomainId);
            
            var authorization = new DomainAuthorization(id, request.Type, request.Properties);
            
            await db.DomainAuthorizations.InsertAsync(authorization);

            return authorization;
        }
        
        public async Task CompleteAsync(DomainAuthorization authorization, DateTime expires)
        {
            Ensure.NotNull(authorization, nameof(authorization));

            await db.DomainAuthorizations.PatchAsync(authorization.Id, new[] {
                Change.Replace("completed", Expression.Func("NOW")),
                Change.Replace("expires", expires),
            }, condition: Expression.IsNull("completed"));
        }

        public async Task RevokeAsync(DomainAuthorization authorization)
        {
            Ensure.NotNull(authorization, nameof(authorization));

            await db.DomainAuthorizations.PatchAsync(authorization.Id, new[] {
                Change.Replace("revoked", Expression.Func("NOW"))
            }, condition: Expression.IsNull("revoked"));
        }
    }
}