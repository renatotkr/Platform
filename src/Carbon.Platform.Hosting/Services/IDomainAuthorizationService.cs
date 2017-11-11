using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Hosting
{
    public interface IDomainAuthorizationService
    {
        Task CompleteAsync(DomainAuthorization authorization, DateTime expires);

        Task<DomainAuthorization> CreateAsync(CreateDomainAuthorizationRequest request);

        Task<DomainAuthorization> GetLatestAsync(IDomain domain);

        Task<IReadOnlyList<DomainAuthorization>> ListAsync(IDomain domain);

        Task RevokeAsync(DomainAuthorization authorization);
    }
}