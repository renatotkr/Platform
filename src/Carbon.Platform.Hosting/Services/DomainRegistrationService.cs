using System;
using System.Threading.Tasks;
using Carbon.Data;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Hosting
{
    public class DomainRegistrationService : IDomainRegistrationService
    {
        private readonly HostingDb db;
        private readonly IDomainService domainService;

        public DomainRegistrationService(HostingDb db, IDomainService domainService)
        {
            this.db            = db ?? throw new ArgumentNullException(nameof(db));
            this.domainService = domainService ?? throw new ArgumentNullException(nameof(domainService));
        }
        
        public async Task<DomainRegistration> GetAsync(long id)
        {
            return await db.DomainRegistrations.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.DomainRegistration, id);
        }

        public async Task<DomainRegistration> CreateAsync(CreateDomainRegistrationRequest request)
        {
            var domain = await domainService.GetAsync(request.DomainId);

            var registrationId = await DomainRegistrationId.NextAsync(db.Context, request.DomainId);

            var registration = new DomainRegistration(
                id          : registrationId,
                domainId    : domain.Id,
                ownerId     : request.OwnerId,
                registrarId : request.RegistrarId,
                expires     : request.Expires
            );

            await db.DomainRegistrations.InsertAsync(registration);

            // Update the domain?

            return registration;
        }

        public async Task UpdateAsync(UpdateDomainRegistrationRequest request)
        {           
            await db.DomainRegistrations.PatchAsync(request.RegistrationId, new[] {
                Change.Replace("expires", request.Expires)
            });
        }
    }
}