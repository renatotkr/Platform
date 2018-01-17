using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Environments
{
    [Dataset("EnvironmentEdges")] // Nodes?
    public class EnvironmentEdge
    {
        public EnvironmentEdge() { }

        public EnvironmentEdge(
            long environmentId,
            int locationId,
            long? distributionId = null)
        {
            Ensure.IsValidId(environmentId, nameof(environmentId));
            Ensure.IsValidId(locationId,    nameof(locationId));

            EnvironmentId = environmentId;
            LocationId    = locationId;
            DistributionId = distributionId;
        }

        [Member("environmentId"), Key]
        public long EnvironmentId { get; }

        [Member("locationId"), Key] // PoPId ?
        public int LocationId { get; }
        
        // note: Borg doesn't use distributions

        [Member("distributionId"), Indexed]
        public long? DistributionId { get; }

        #region Timestamps

        // May be set in the future to temporary take an edge node offline for maintenance
        [Member("activated")]
        public DateTime? Activated { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}