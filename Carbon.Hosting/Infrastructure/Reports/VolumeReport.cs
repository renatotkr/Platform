namespace Carbon.Platform
{
    using Data.Annotations;

    [Dataset("VolumeReports")]
    public class VolumeReport
    {
        [Member(1), Key]
        public string VolumeId { get; set; }

        [Member(2), Key]
        public long Id { get; set; }

        /// <summary>
        /// The number of bytes available at the end of the reporting period
        /// </summary>
        [Member(3)]
        public long Available { get; set; }

        /// <summary>
        /// The capacity (in bytes) of the drive
        /// </summary>
        [Member(4)]
        public long Size { get; set; }
       
        [Member(5)] // in bytes per second
        public int ReadRate { get; set; }
      
        [Member(6)] // in bytes per second
        public int WriteRate { get; set; }

        /// <summary>
        /// The percentage of total time the drive was busy servicing read operations
        /// </summary>
        [Member(7)]
        public float ReadTime { get; set; }

        /// <summary>
        /// The percentage of total time the drive was busy servicing write operations
        /// </summary>
        [Member(8)]
        public float WriteTime { get; set; }
    }
}