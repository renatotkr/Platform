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
            Ensure.Id(ownerId,                nameof(ownerId));
            Ensure.Id(environmentId,          nameof(environmentId));
            Ensure.Id(providerId,             nameof(providerId));
            Ensure.NotNullOrEmpty(resourceId, nameof(resourceId));

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