namespace Carbon.Platform.Environments
{
    public class CreateEnvironmentEdgeRequest
    {
        public CreateEnvironmentEdgeRequest(
            long environmentId,
            int locationId,
            long? distributionId = null)
        {
            Ensure.Id(environmentId, nameof(environmentId));
            Ensure.Id(locationId,        nameof(locationId));

            EnvironmentId = environmentId;
            LocationId = locationId;
            DistributionId = distributionId;
        }

        public long EnvironmentId { get; }

        public int LocationId { get; }

        public long? DistributionId { get; }
    }
}