using System;
using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    [Dataset("HostPrograms")]
    public class HostProgram : IProgram
    {
        public HostProgram() { }

        public HostProgram(
            long hostId, 
            long programId, 
            string programName,
            SemanticVersion programVersion, 
            string[] addresses = null,
            JsonObject properties = null)
        {
            #region Preconditions

            if (hostId <= 0)
                throw new ArgumentException("Must be > 0", nameof(hostId));

            if (programId <= 0)
                throw new ArgumentException("Must be > 0", nameof(programId));

            #endregion

            HostId         = hostId;
            ProgramId      = programId;
            ProgramName    = programName;
            ProgramVersion = programVersion;
            Addresses      = addresses;
            Properties     = properties;
        }

        [Member("hostId"), Key]
        public long HostId { get; }
        
        [Member("programId"), Key]
        public long ProgramId { get; }
        
        [Member("programName")]
        [StringLength(100)]
        public string ProgramName { get; }

        [Member("programVersion")]
        public SemanticVersion ProgramVersion { get; }

        // e.g. http://*:80

        [Member("addresses")]
        [StringLength(200)]
        public string[] Addresses { get; }

        [Member("runtime")]
        [StringLength(50)]
        public string Runtime { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region Health

        [Member("heartbeat")]
        public DateTime? Heartbeat { get; }

        [Member("requestCount")]
        public long RequestCount { get; }

        [Member("errorCount")]
        public long ErrorCount { get; }

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion

        #region IProgram

        long IProgram.Id => ProgramId;

        string IProgram.Name => ProgramName;

        SemanticVersion IProgram.Version => ProgramVersion;

        #endregion
    }
}
