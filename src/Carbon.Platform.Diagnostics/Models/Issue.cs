using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.Platform.Diagnostics
{
    [Dataset("Issues", Schema = "Diagnostics")]
    public class Issue : IIssue
    {
        public Issue() { }

        public Issue(
            long id, 
            IssueType type = IssueType.Unknown, 
            int? locationId = null,
            string description = null,
            string externalId = null,
            JsonObject details = null)
        {
            Id          = id;
            Type        = type;
            LocationId  = locationId;
            Description = description;
            ExternalId  = externalId;
            Properties     = details ?? new JsonObject();
        }

        // environmentId | #
        [Member("id"), Key]
        public long Id { get; }

        [Member("type")]
        public IssueType Type { get; }

        [Member("locationId")]
        public int? LocationId { get; }

        [Member("description")]
        [StringLength(1000)]
        public string Description { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        // e.g. github:issue/1
        [Member("externalId")]
        [StringLength(100)]
        public string ExternalId { get; }

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

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