using System;

namespace Carbon.Platform.Hosting
{
    public class UpdateDomainRegistrationRequest
    {
        public UpdateDomainRegistrationRequest(long registrationId, DateTime expires)
        {
            Validate.Id(registrationId, nameof(registrationId));

            RegistrationId = registrationId;
            Expires = expires;
        }

        public long RegistrationId { get; }

        public DateTime Expires { get; }
    }
}