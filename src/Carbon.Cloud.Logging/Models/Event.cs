using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Cloud.Logging
{
    [Dataset("Events", Schema = "Logs")]
    public class Event
    {
        public Event() { }

        public Event(
            string action,
            string resource,
            long? userId = null,
            string source = null,
            JsonObject context = null,
            JsonObject properties = null)
        {
            Action     = action   ?? throw new ArgumentNullException(nameof(action));
            Resource   = resource ?? throw new ArgumentNullException(nameof(resource));
            Context    = context;
            Properties = properties;
            Source     = source;
            UserId     = userId;
        }

        // accountId | timestamp | #
        [Member("id"), Key]
        [DataMember(Order = 1)]
        public Uid Id { get; set; }

        [Member("action")]
        [Ascii, StringLength(100)]
        [DataMember(Order = 2)]
        public string Action { get; } // View, Invoke, ...

        [Member("resource"), Indexed]
        [StringLength(150)]
        [DataMember(Order = 3)]
        public string Resource { get; }

        // origin?
        [Member("source"), Optional]
        [StringLength(120)]
        [DataMember(Order = 4)]
        public string Source { get; }

        [Member("properties"), Optional]
        [StringLength(1000)]
        [DataMember(Order = 5)]
        public JsonObject Properties { get; }

        [Member("context"), Optional]
        [StringLength(1000)]
        [DataMember(Order = 6)]
        public JsonObject Context { get; }

        [Member("userId"), Indexed]
        [DataMember(Order = 7)]
        public long? UserId { get; }
        
        // SessionId?

        #region Helpers

        [IgnoreDataMember]
        public DateTime Created => RequestId.GetTimestamp(Id);

        #endregion
    }
}

// ACTION  | RESOURCE
// launch  | carbon:host/1
// restart | carbon:host/1
// view    | https://carbon/