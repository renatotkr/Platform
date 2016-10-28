using System;

namespace Carbon.Computing
{
    using Data.Annotations;
    using Networking;

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

        [Member(1), Identity]
        public long Id { get; set; }

        [Member(2), Indexed]
        public long ProgramId { get; set; }

        [Member(3), Indexed]
        public long HostId { get; set; }

        [Member(4), Optional] // if running inside a docker container
        public string ContainerId { get; set; } // 8fddbcbb101c

        [Member(5), Indexed] // if servicing a backend
        public long? BackendId { get; set; }

        [Member(6)]
        public int PID { get; set; } // https://en.wikipedia.org/wiki/Process_identifier

        [Member(7)]
        public string Name { get; set; }

        [Member(8)] 
        public Semver ProgramVersion { get; set; }

        [Member(9), Mutable] // a code indication weather we succesfully exited or not...
        public int? ExitStatus { get; set; }

        [Member(10)]
        public NetworkPortList Ports { get; set; }

        [Member(12), Timestamp]
        public DateTime Created { get; set; }

        #region Stats

        // CPU Cycles / load

        [Member(20), Mutable]
        public long CallCount { get; set; }

        [Member(21), Mutable]
        public long MemoryUsed { get; set; }

        [Member(22), Mutable]
        public long DataIn { get; } // egress?

        [Member(23), Mutable]
        public long DataOut { get; }

        #endregion
    }
}

// IsolationLevel: PhysicalHost, VirtualHost, Container

// In docker, we may need to assign dynamic ports to processes if we're sharing a single IP address on the host.

// Permissions
// ResourceLimits

// An instance of a computer program that is being executed