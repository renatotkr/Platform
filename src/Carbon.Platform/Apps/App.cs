using System;

namespace Carbon.Platform.Apps
{
    using Data.Annotations;
    using Json;
    using Versioning;

    [Dataset("Apps")]
    public class App : IApp
    {
        public App() { }

        public App(string name, AppType type)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            Name = name;
            Type = type;
        }

        [Member("id"), Identity]
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

        [Member("created")]
        public DateTime Created { get; set; }

        [Member("modified"), Timestamp]
        public DateTime Modified { get; set; }

        #region Helpers

        public AppStatus Status { get; set; } = AppStatus.Running;

        // public IList<AppInstance> Instances { get; set; }

        // public IList<AppRelease> Releases { get; set; }

        #endregion

        // name@1.2.1
        public override string ToString() => Name + "@" + Version;
    }
}

/*
 
{
   listener: "http://*:80",
   machineType: "t2.xlarge"
   ...
}

 */

// An backend/app exposes a program as a load balanced worker or web service

// Web Role
// - spawns one or more instances to handle user requests (autoscaling) [using containers, as needed]
// - hosted behind a load balancer

// bindings (host, protocal, port)

// Worker Role
// - Scales to queue...