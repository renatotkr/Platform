using System;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Dataset("Outages")]
    public class Outage 
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("locationId")]
        public long LocationId { get; set; }

        [Member("scope")] // Effected services
        public ResourceType Scope { get; set; }

        [Member("resolved")]
        public DateTime? Resolved { get; set; }

        [Member("created")]
        public DateTime Created { get; set; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }

        #region IResource

        public CloudProvider Provider => LocationHelper.GetProvider(LocationId);

        #endregion
    }
}