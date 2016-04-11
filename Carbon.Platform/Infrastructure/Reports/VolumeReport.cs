using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carbon.Platform
{
    [Table("VolumeReports")]
    public class VolumeReport
    {
        [Column("vid"), Key]
        public string VolumeId { get; set; }

        [Column("rid"), Key]
        public long ReportId { get; set; }

        /// <summary>
        /// The number of bytes available at the end of the reporting period
        /// </summary>
        [Column("a")]
        public long Available { get; set; }

        /// <summary>
        /// The capacity (in bytes) of the drive
        /// </summary>
        [Column("s")]
        public long Size { get; set; }
       
        [Column("rr")] // in bytes per second
        public int ReadRate { get; set; }
      
        [Column("wr")] // in bytes per second
        public int WriteRate { get; set; }

        /// <summary>
        /// The percentage of total time the drive was busy servicing read operations
        /// </summary>
        [Column("rt")]
        public float ReadTime { get; set; }

        /// <summary>
        /// The percentage of total time the drive was busy servicing write operations
        /// </summary>
        [Column("wt")]
        public float WriteTime { get; set; }
    }
}