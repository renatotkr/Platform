using System;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;
    using Versioning;

    [Dataset("Processes")]
    public class Process
    {
        public Process() { }

        public Process(IProgram program, IHost host)
        {
            #region Preconditions

            if (program == null)
                throw new ArgumentNullException(nameof(program));

            if (host == null)
                throw new ArgumentNullException(nameof(host));

            #endregion

            ProgramId = program.Id;
            HostId    = host.Id;
        }

        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("programId"), Indexed]
        public long ProgramId { get; set; }

        [Member("hostId"), Indexed]
        public long HostId { get; set; }

        [Member("containerId")] // if running inside a docker container
        public long? ContainerId { get; set; } // 8fddbcbb101c

        [Member("pid")]
        public int PID { get; set; } // https://en.wikipedia.org/wiki/Process_identifier

        [Member("name")]
        public string Name { get; set; }

        [Member("programVersion")] 
        public SemanticVersion ProgramVersion { get; set; }

        [Member("exitStatus"), Mutable] // a code indication weather we succesfully exited or not...
        public int? ExitStatus { get; set; }

        [Member("created"), Timestamp]
        public DateTime Created { get; set; }
    }
}

// IsolationLevel: PhysicalHost, VirtualHost, Container

// In docker, we may need to assign dynamic ports to processes if we're sharing a single IP address on the host.

// Permissions
// ResourceLimits

// An instance of a computer program that is being executed


// Metrics
// CPUUtilization
// callcount
// memoryused
// networkin
// networkout