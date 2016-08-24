using System;

namespace Carbon.Platform
{
    using Data.Annotations;
    using Networking;

    [Record(TableName = "Processes")]
    public class ProcessInfo
    {
        public ProcessInfo() { }

        public ProcessInfo(IProgram program, IHost host)
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

        [Member(3)] 
        public Semver Version { get; set; } // program version

        [Member(4)]
        public string Slug { get; set; }    // program slug

        [Member(5), Indexed]
        public long HostId { get; set; }

        [Member(6), Indexed] // if servicing a backend
        public long? BackendId { get; set; }

        [Member(7)]
        public int PID { get; set; } // https://en.wikipedia.org/wiki/Process_identifier

        [Member(8), Optional] // if running inside a docker container
        public string ContainerId { get; set; } // 8fddbcbb101c

        [Member(9, mutable: true)] // a code indication weather we succesfully exited or not...
        public int? ExitStatus { get; set; }

        [Member(10)]
        public NetworkPortList Ports { get; set; }

        [Member(11), Timestamp(false)]
        public DateTime Created { get; set; }

        #region Stats

        // CPU Cycles / load

        [Member(12, mutable: true)]
        public long CallCount { get; set; }

        [Member(13, mutable: true)]
        public long MemoryUsed { get; set; }

        [Member(14, mutable: true)]
        public long DataIn { get; } // egress?

        [Member(15, mutable: true)]
        public long DataOut { get; }

        #endregion
    }
}

// IsolationLevel: PhysicalHost, VirtualHost, Container

// In docker, we may need to assign dynamic ports to processes if we're sharing a single IP address on the host.

// Permissions
// ResourceLimits

// An instance of a computer program that is being executed