using System;

namespace Carbon.Platform.Hosting
{
    public class CreateDomainRegistrationRequest
    {
        public CreateDomainRegistrationRequest(
            long domainId,
            long ownerId,
            long registrarId,
            DateTime expires)
        {
            Ensure.IsValidId(domainId, nameof(domainId));
            Ensure.IsValidId(ownerId, nameof(ownerId));
            Ensure.IsValidId(registrarId, nameof(registrarId));

            DomainId    = domainId;
            OwnerId     = ownerId;
            RegistrarId = registrarId;
            Expires     = expires;
        }

        public long DomainId { get; }

        public long OwnerId { get; }

        public long RegistrarId { get; }

        public DateTime Expires { get; }

        // FromWhois?
    }
}