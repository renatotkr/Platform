using System;

namespace Carbon.Platform.Hosting
{
    public struct UpdateDomainRegistrationRequest
    {
        public UpdateDomainRegistrationRequest(
            long registrationId,
            DateTime expires)
        {
            RegistrationId = registrationId;
            Expires = expires;
        }

        public long RegistrationId { get; }

        public DateTime Expires { get; }
    }
}