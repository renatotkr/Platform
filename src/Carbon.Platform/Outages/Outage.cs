using System;
using System.Runtime.Serialization;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Dataset("Outages")]
    public class Outage : IOutage
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("locationId")]
        public long LocationId { get; set; }

        // bucket,host
        [Member("scope")] // bucket|host
        public string[] Scope { get; set; }

        [Member("resolved")]
        public DateTime? Resolved { get; set; }

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        #region IResource

        [IgnoreDataMember]
        public ResourceProvider Provider => LocationHelper.GetProvider(LocationId);

        #endregion
    }
}