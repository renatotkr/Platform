using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Platform.Diagnostics
{
    [Dataset("Exceptions", Schema = "Diagnostics")]
    public class EnvironmentException : IException
    {
        // environmentId | timestamp/ms | sequenceNumber
        [Member("id"), Key]
        public BigId Id { get; set; }

        [Member("hostId")]
        public long HostId { get; set; }

        [Member("type")]
        [MaxLength(1000)]
        public string Type { get; set; }

        [Member("message")]
        [StringLength(200)]
        public string Message { get; set; }
        
        // [ { file, line, function } ]

        [Member("stackTrace"), Optional]
        [StringLength(2000)]
        public string StackTrace { get; set; }
        
        // appVersion, ...
        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        // userAgent, url, etc.
        [Member("context")]
        [StringLength(1000)]
        public JsonObject Context { get; set; }

        [Member("issueId")]
        [Indexed]
        public long? IssueId { get; set; }

        [Member("sessionId"), Optional]
        public long? SessionId { get; set; }

        [Member("clientId")]
        public long? ClientId { get; set; }

        #region Helpers

        [IgnoreDataMember]
        public string[] Stack => StackTrace?.Split('\n');

        [IgnoreDataMember]
        public string Url => Context["url"];

        [IgnoreDataMember]
        public string HttpMethod => Context["httpMethod"] ?? "GET";

        #endregion
    }
}