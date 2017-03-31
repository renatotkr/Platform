using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Apps
{
    using Data.Annotations;
    using Json;
    using Protection;
    using Versioning;

    [Dataset("Apps")]
    public class App : IApp
    {
        public App() { }

        public App(long id, SemanticVersion version)
        {
            Id = id;
            Version = version;
        }

        public App(long id, string name, AppType type)
        {
            Id   = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
        }

        [Member("id"), Key]
        public long Id { get; set; }

        [Member("version")] // deployed version
        public SemanticVersion Version { get; set; }

        [Member("type")] // Webapp | Worker
        public AppType Type { get; set; }

        [Member("name"), Unique]
        [StringLength(50)]
        public string Name { get; set; }

        [Member("env"), Mutable] // JSON encoded environment data
        [StringLength(2000)]
        public JsonObject Env { get; set; }

        [Member("source")]
        [StringLength(100)]
        public string Source { get; set; }
        
        [Member("digest")]
        public Hash Digest { get; set; }
        
        [Member("created"), Timestamp]
        public DateTime Created { get; set; }
        
        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        // name@1.2.1
        public override string ToString()
        {
            return Name + "@" + Version;
        }
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