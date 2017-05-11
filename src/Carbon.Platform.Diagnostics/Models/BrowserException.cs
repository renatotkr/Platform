using System.Runtime.Serialization;

using Carbon.Data.Sequences;

namespace Carbon.Platform.Diagnostics
{
    using Data.Annotations;
    using Json;

    [Dataset("BrowserExceptions")]
    public class BrowserException : IException
    {
        // appId + timestamp + sequence
        [Member("id"), Key]
        public BigId Id { get; set; }

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
        
        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        // userAgent, url, etc.
        [Member("context")]
        [StringLength(1000)]
        public JsonObject Context { get; set; }

        [Member("issueId"), Indexed]
        public long? IssueId { get; set; }
        
        [Member("sessionId")]
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