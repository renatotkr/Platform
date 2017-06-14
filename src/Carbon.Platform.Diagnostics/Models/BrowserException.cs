
using Carbon.Data.Sequences;

namespace Carbon.Platform.Diagnostics
{
    using Data.Annotations;
    using Json;

    [Dataset("BrowserExceptions", Schema = "Diagnostics")]
    public class BrowserException : IException
    {
        // environmentId + timestamp + sequence
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
        
        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; set; }

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
    }
}