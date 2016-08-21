using System;
using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform
{
    using Data.Annotations;

    // An instance of a computer program that is being executed

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

        [Identity]
        public long Id { get; set; }
       
        [Indexed]
        public long ProgramId { get; set; }

        public Semver ProgramVersion { get; set; }

        [Indexed]
        public long HostId { get; set; }

        [Mutable] // a code indication weather we succesfully exited or not...
        public int? ExitStatus { get; set; }

        [Indexed, Optional] // if servicing a backend
        public long? BackendId { get; set; }

        int PID { get; set; } // https://en.wikipedia.org/wiki/Process_identifier

        [Timestamp]
        public DateTime Created { get; set; }

        #region Stats

        // CPU Cycles / load

        [Mutable]
        public long MemoryUsed { get; set; }

        [Mutable]
        public long CallCount { get; set; }

        [Mutable]
        public long DataIn { get; }

        [Mutable]
        public long DataOut { get; }

        #endregion
    }
}


// When a process dies, do we automatically respawn a new one -- or is this a higher level (i.e. backend) concern

// name / slug?