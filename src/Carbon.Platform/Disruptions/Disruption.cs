using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Disruptions
{
    using Data.Annotations;

    [Dataset("Disruptions")]
    public class Disruption : IDisruption
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("locationId")]
        public long LocationId { get; set; }

        [Member("serviceType")]
        public ResourceType ServiceType { get; set; }

        [Member("description")]
        [StringLength(200)]
        public string Description { get; set; }

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [Member("resolved")]
        public DateTime? Resolved { get; set; }

        #endregion
    }
}