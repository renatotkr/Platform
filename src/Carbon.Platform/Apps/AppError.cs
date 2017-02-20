using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Apps
{
    using Data.Annotations;
    using Json;

    [Dataset("AppErrors")]
    public class AppError
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("appId")]
        [Indexed]
        public long AppId { get; set; }

        [Member("appVersion")]  // revision?
        [StringLength(50)]
        public string AppVersion { get; set; }

        [Member("hostId")]
        public long HostId { get; set; }

        [Member("type")]
        [MaxLength(1000)]
        public string Type { get; set; }

        [Member("message")]
        [StringLength(200)]
        public string Message { get; set; }

        // [Optional]
        [Member("stackTrace")]
        [StringLength(2000)]
        public string StackTrace { get; set; }

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        // userAgent, url, etc.
        [Member("context")]
        [StringLength(1000)]
        public JsonObject Context { get; set; }

        [Member("userId"), Optional]
        [Indexed] // sparse
        public long? UserId { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        #region Helpers

        [IgnoreDataMember]
        public string[] Stack => StackTrace.Split('\n');

        [IgnoreDataMember]
        public string Url => Context["url"];

        [IgnoreDataMember]
        public string HttpMethod => Context["httpMethod"] ?? "GET";

        #endregion

    }
}