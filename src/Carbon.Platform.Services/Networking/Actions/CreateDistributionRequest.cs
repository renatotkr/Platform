using Carbon.Json;

namespace Carbon.Platform.Networking
{
    public class CreateDistributionRequest
    {
        public CreateDistributionRequest(
            long ownerId,
            long environmentId,
            int providerId,
            string resourceId,
            JsonObject properties = null)
        {
            Validate.Id(ownerId,                nameof(ownerId));
            Validate.Id(environmentId,          nameof(environmentId));
            Validate.Id(providerId,             nameof(providerId));
            Validate.NotNullOrEmpty(resourceId, nameof(resourceId));

            OwnerId       = ownerId;
            EnvironmentId = environmentId;
            Properties    = properties;
            ProviderId    = providerId;
            ResourceId    = resourceId;
        }

        public long OwnerId { get; }

        public long EnvironmentId { get; }
       
        public JsonObject Properties { get; }

        public int ProviderId { get; }

        public string ResourceId { get; }
    }
}