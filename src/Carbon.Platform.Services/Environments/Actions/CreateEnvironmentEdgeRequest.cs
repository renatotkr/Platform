namespace Carbon.Platform.Environments
{
    public class CreateEnvironmentEdgeRequest
    {
        public CreateEnvironmentEdgeRequest(
            long environmentId,
            int locationId,
            long? distributionId = null)
        {
            Validate.Id(environmentId, nameof(environmentId));
            Validate.Id(locationId,        nameof(locationId));

            EnvironmentId = environmentId;
            LocationId = locationId;
            DistributionId = distributionId;
        }

        public long EnvironmentId { get; }

        public int LocationId { get; }

        public long? DistributionId { get; }
    }
}