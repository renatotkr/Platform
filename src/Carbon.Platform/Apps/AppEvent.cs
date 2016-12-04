using System;

namespace Carbon.Platform.Apps
{
    using Data.Annotations;
    using Versioning;

    [Dataset("AppEvents")]
    public class AppEvent
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("appId"), Indexed]
        public long AppId { get; set; }

        [Member("hostId")]
        public long? HostId { get; set; }

        [Member("appVersion")]
        public SemanticVersion AppVersion { get; set; }

        [Member("message")]
        public string Message { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }
    }

    // AddedInstanced
    // RemovedInstance
    // UpdatedRelease
}
