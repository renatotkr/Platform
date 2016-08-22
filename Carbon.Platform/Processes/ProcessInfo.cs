using System;

namespace Carbon.Platform
{
    using Data.Annotations;

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
        public string Slug { get; set; }    // program slug

        [Member(4)] 
        public Semver Version { get; set; } // program version

        [Member(5), Indexed]
        public long HostId { get; set; }

        [Member(6), Indexed, Optional] // if servicing a backend
        public long? BackendId { get; set; }

        [Member(7)]
        public int PID { get; set; } // https://en.wikipedia.org/wiki/Process_identifier

        [Member(8, Mutable = true)] // a code indication weather we succesfully exited or not...
        public int? ExitStatus { get; set; }

        [Member(8), Version]
        public DateTime Created { get; set; }

        #region Stats

        // CPU Cycles / load

        [Member(10, Mutable = true)]
        public long CallCount { get; set; }

        [Member(11, Mutable = true)]
        public long MemoryUsed { get; set; }

        [Member(12, Mutable = true)]
        public long DataIn { get; }

        [Member(13, Mutable = true)]
        public long DataOut { get; }

        #endregion
    }
}

// Permissions
// ResourceLimits

// An instance of a computer program that is being executed

// When a process dies, do we automatically respawn a new one -- or is this a higher level (i.e. backend) concern