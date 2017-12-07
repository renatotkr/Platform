using Carbon.Json;

namespace Carbon.Platform.Hosting
{
    public class CreateDomainAuthorizationRequest
    {
        public CreateDomainAuthorizationRequest(
            long domainId,
            DomainAuthorizationType type, 
            JsonObject properties)
        {
            Validate.Id(domainId, nameof(domainId));

            DomainId   = domainId;
            Type       = type;
            Properties = properties;
        }

        public long DomainId { get; }

        public DomainAuthorizationType Type { get; }

        public JsonObject Properties { get; }
    }
}