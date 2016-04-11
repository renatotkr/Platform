using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.Serialization;

namespace Carbon.Platform
{
    [Table("Volumes")]
    public class VolumeInfo
    {
        [Column("mid"), Key]
        [DataMember(Name = "machineId")]
        public int MachineId { get; set; }

        /// <summary>
        /// e.g. D
        /// </summary>
        [Column("name"), Key]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Column("status")]
        [DataMember(Name = "status")]
        public DeviceStatus Status { get; set; }

        [Column("available")]
        [DataMember(Name = "available")]
        public long Available { get; set; }

        [Column("used")]
        [DataMember(Name = "used")]
        public long Used { get; set; }

        [Column("size")]
        [DataMember(Name = "size")]
        public long Size { get; set; }

        [IgnoreDataMember]
        public string Id => MachineId + "/" + Name;
        
        [IgnoreDataMember]
        public DriveInfo Drive { get; set; }        
    }
}