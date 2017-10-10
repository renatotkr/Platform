using System;
using System.Threading.Tasks;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Hosting
{
    public class DomainRegistrationService : IDomainRegistrationService
    {
        private readonly HostingDb db;

        public DomainRegistrationService(HostingDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        public async Task<DomainRegistration> GetAsync(long id)
        {
            return await db.DomainRegistrations.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.DomainRegistration, id);
        }

        public async Task<DomainRegistration> CreateAsync(CreateDomainRegistrationRequest request)
        {
            var registration = new DomainRegistration(
                id          : await db.DomainRegistrations.Sequence.NextAsync(), // scopedId?
                domainId    : request.DomainId,
                ownerId     : request.OwnerId,
                registrarId : request.RegistrarId,
                expires     : request.Expires
            );

            await db.DomainRegistrations.InsertAsync(registration);

            return registration;
        }
    }
}