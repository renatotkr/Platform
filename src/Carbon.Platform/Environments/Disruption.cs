using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Disruptions
{
    using Data.Annotations;

    [Dataset("Disruptions")]
    public class Disruption : IDisruption
    {
        public Disruption() { }

        public Disruption(long id, long locationId, string description = null)
        {
            Id = id;
            LocationId = locationId;
            Description = description;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("locationId")]
        public long LocationId { get; }

        [Member("description")]
        [StringLength(200)]
        public string Description { get; }

        [Member("environmentId")]
        public long EnvironmentId => ScopedId.GetScope(Id);

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