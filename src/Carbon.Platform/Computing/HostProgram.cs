using System;
using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    [Dataset("HostPrograms")]
    public class HostProgram
    {
        public HostProgram() { }

        public HostProgram(
            long hostId, 
            long programId, 
            SemanticVersion programVersion, 
            int? port = null,
            JsonObject configuration = null,
            JsonObject properties = null)
        {
            #region Preconditions

            if (hostId <= 0)
                throw new ArgumentException("Must be > 0", nameof(hostId));

            if (programId <= 0)
                throw new ArgumentException("Must be > 0", nameof(programId));

            #endregion

            HostId = hostId;
            ProgramId      = programId;
            ProgramVersion = programVersion.ToString();
            Port           = port;
            Configuration  = configuration;
            Properties     = properties;
        }

        [Member("hostId"), Key]
        public long HostId { get; }
        
        [Member("programId"), Key]
        public long ProgramId { get; }

        [Member("programVersion")]
        [StringLength(100)]
        public string ProgramVersion { get; }

        [Member("port")]
        public int? Port { get; }

        [Member("configuration")]
        [StringLength(1000)]
        public JsonObject Configuration { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}
