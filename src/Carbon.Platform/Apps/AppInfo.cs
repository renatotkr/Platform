using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.Platform.Apps
{
    [Dataset("Apps")]
    public class AppInfo : IApp
    {
        public AppInfo() { }

        public AppInfo(long id, string name, AppType type)
        {
            Id   = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("type")] // Webapp | Worker
        public AppType Type { get; }

        [Member("name"), Unique]
        [StringLength(63)]
        public string Name { get; }

        // TODO: Move this to the app enviornments variables
        [Member("env"), Mutable] // JSON encoded environment data
        [StringLength(2000)]
        [Obsolete]
        public JsonObject Env { get; set; }

        [Member("source")]
        [StringLength(100)]
        public string Source { get; set; }

        [Member("repositoryId")]
        public long RepositoryId { get; set; }

        [Member("ownerId")] // May change
        public long OwnerId { get; set; }

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}

/*
 
{
   listener: "http://*:80",
   machineType: "t2.xlarge",
   framework: "nodejs@10.x",
   entryPoint: "funcName"
   ...
}

 */