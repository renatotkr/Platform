using System.Runtime.Serialization;

namespace Carbon.Platform
{
    [DataContract]
    public class VolumeStats
    {
        [DataMember(Order = 1)]
        public long Size { get; set; }

        [DataMember(Order = 2)]
        public long Available { get; set; }

        [DataMember(Order = 3)]
        public long BytesRead { get; set; }

        [DataMember(Order = 4)]
        public long BytesWritten { get; set; }

        /// <summary>
        /// The percentage the volume was busy reading
        /// </summary>
        [DataMember(Order = 5)]
        public float ReadTime { get; set; }

        /// <summary>
        /// Percentage the volume was busy writing
        /// </summary>
        [DataMember(Order = 6)]
        public float WriteTime { get; set; }
    }
}