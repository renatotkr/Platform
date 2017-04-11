using System.Runtime.Serialization;

namespace Carbon.Platform.Diagnostics
{
    using Data.Annotations;
    using Identity;
    using Json;

    [Dataset("ServerExceptions")]
    public class ServerException : IException
    {
        // appEnvironmentId | timestamp | sequence
        [Member("id"), Key]
        public BigId Id { get; set; }

        [Member("hostId")]
        public long HostId { get; set; }

        // revision
        [Member("appVersion")]
        [StringLength(50)]
        public string AppVersion { get; set; }

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
        
        [Member("sessionId"), Optional]
        [Indexed] // sparse
        public long? SessionId { get; set; }

        
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