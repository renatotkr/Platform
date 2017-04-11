using System.Runtime.Serialization;

namespace Carbon.Platform
{
    [DataContract]
    public class VolumeStats
    {
        [DataMember(Order = 1)]
        public long TotalBytes { get; set; }

        [DataMember(Order = 2)]
        public long AvailableBytes { get; set; }

        [DataMember(Order = 3)]
        public long ReadBytes { get; set; }

        [DataMember(Order = 4)]
        public long WriteBytes { get; set; }

        [DataMember(Order = 5, EmitDefaultValue = false)]
        public long ReadOperations { get; set; }

        [DataMember(Order = 6, EmitDefaultValue = false)]
        public long WriteOperations { get; set; }

        /// <summary>
        /// The total number of seconds the volume was busy reading
        /// </summary>
        [DataMember(Order = 7)]
        public long TotalReadTime { get; set; }

        /// <summary>
        /// The total number of seconds the volume was busy writing
        /// </summary>
        [DataMember(Order = 8)]
        public long TotalWriteTime { get; set; }

        /// <summary>
        /// The number of seconds the volume was idle (no read or write operations)
        /// </summary>
        [DataMember(Order = 9, EmitDefaultValue = false)]
        public long TotalIdleTime { get; set; }
    }
}