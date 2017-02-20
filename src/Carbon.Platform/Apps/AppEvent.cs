using System;

namespace Carbon.Platform.Apps
{
    using Data.Annotations;
    using Json;
    using Versioning;

    [Dataset("AppEvents")]
    public class AppEvent
    {
        public AppEvent() { }

        public AppEvent(IApp app, AppEventType type, string message = null)
        {
            #region Preconditions

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            #endregion

            AppId = app.Id;
            AppVersion = app.Version;
            Type = type;
            Message = message;
        }

        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("type")]
        public AppEventType Type { get; set; }

        [Member("appId"), Indexed]
        public long AppId { get; set; }

        [Member("hostId"), Indexed, Optional]
        public long? HostId { get; set; }

        [Member("appVersion"), Optional]
        public SemanticVersion? AppVersion { get; set; }

        [Member("message"), Optional]
        public string Message { get; set; }

        [Member("details"), Optional]
        public JsonObject Details { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }
    }

    public enum AppEventType
    {
        Created   = 1,
        Built     = 2,
        Released  = 3,  // New Version
        Deployed  = 4,  // On instance
        Activated = 5,  // On instance
        Reloaded  = 6,

        AddedInstance   = 10,
        RemovedInstance = 11,
    }
}