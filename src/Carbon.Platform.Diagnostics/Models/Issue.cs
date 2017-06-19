using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Platform.Diagnostics
{
    [Dataset("Issues", Schema = "Diagnostics")]
    public class Issue : IIssue
    {
        public Issue() { }

        public Issue(
            BigId id, 
            IssueType type = IssueType.Unknown, 
            string description = null)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            #endregion

            Id          = id;
            Type        = type;
            Description = description;
        }

        // environmentId | timestamp/ms | #
        [Member("id"), Key]
        public BigId Id { get; }

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

        // i.e. GitHub issue...

        [Member("externalId")]
        [StringLength(50)]
        public string ExternalId { get; set; }

        #region Timestamps

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [Member("resolved"), Mutable]
        public DateTime? Resolved { get; set; }

        #endregion
    }

    public enum IssueType
    {
        Unknown = 0
    }
}