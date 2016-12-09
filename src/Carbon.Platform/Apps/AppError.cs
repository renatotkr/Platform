using System;

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
        public string AppVersion { get; set; }

        [Member("hostId")]
        public long HostId { get; set; }

        [Member("type")]
        [MaxLength(1000)]
        public string Type { get; set; }

        [Member("message")]
        public string Message { get; set; }

        // [Optional]
        [Member("stackTrace")]
        public string StackTrace { get; set; }

        // [Optional]
        [Member("innerType")]
        public string InnerType { get; set; }

        // [Optional]
        [Member("innerMessage")]
        public string InnerMessage { get; set; }

        // [Optional]
        [Member("innerStackTrace")]
        public string InnerStackTrack { get; set; }

        // userAgent, url, etc.

        [Member("context")]
        public JsonObject Context { get; set; }

        [Member("userId")]
        [Indexed] // sparse
        public long? UserId { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }
    }
}