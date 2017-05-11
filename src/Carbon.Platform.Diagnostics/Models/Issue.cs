using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.Platform.Diagnostics
{
    [Dataset("Issues")]
    public class Issue : IIssue
    {
        public Issue() { }

        public Issue(long id, string description = null)
        {
            Id = id;
            Description = description;
        }

        // too small...

        // environmentId + sequence
        [Member("id"), Key]
        public long Id { get; }

        [Member("type")]
        public IssueType Type { get; set; }

        [Member("locationId")]
        public int? LocationId { get; set; }

        [Member("description")]
        [StringLength(1000)]
        public string Description { get; }

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

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

    public enum IssueType
    {
        Unknown = 0
    }
}