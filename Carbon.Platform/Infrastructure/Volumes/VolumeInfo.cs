namespace Carbon.Platform
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Text;

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

        [Column("guid")]
        public string Guid { get; set; }

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
        public DriveInfo DriveInfo { get; set; }

        public string GetInstanceName()
        {
            return DriveInfo.Name.Substring(0, 2);
        }

        [IgnoreDataMember]
        public string FullName => MachineId + "/" + Name;

        public VolumeObserver GetObserver() => new VolumeObserver(this);

        public void Refresh()
        {
            #region Preconditions

            if (DriveInfo == null) throw new ArgumentNullException("The underlying drive info has not been set");

            #endregion

            var di = DriveInfo;

            Available = di.AvailableFreeSpace;
            Size = di.TotalSize;
            Used = di.TotalSize - DriveInfo.TotalFreeSpace;
        }

        public static VolumeInfo FromDriveInfo(DriveInfo drive)
        {
            var v = new VolumeInfo {
                MachineId = Machine.GetAsync().Result.Id,
                Name = drive.Name.Substring(0, 1),
                Available = drive.AvailableFreeSpace,
                Size = drive.TotalSize,
                Used = drive.TotalSize - drive.TotalFreeSpace,
                DriveInfo = drive
            };


            // \\?\Volume{982c634e-e72f-11e1-be66-806e6f6e6963}\

            try
            {
                var id = GetVolumeNameFromMountPoint(drive.RootDirectory.ToString()).Replace("\\\\?\\Volume{", "").TrimEnd('\\', '}');

                v.Guid = id;
            }
            catch { }

            return v;
        }

        public static string GetVolumeNameFromMountPoint(string m)
        {
            const int MaxVolumeNameLength = 100;

            var sb = new StringBuilder(MaxVolumeNameLength);

            if (!GetVolumeNameForVolumeMountPoint(m, sb, (uint)MaxVolumeNameLength))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            return sb.ToString();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetVolumeNameForVolumeMountPoint(string lpszVolumeMountPoint, [Out] StringBuilder lpszVolumeName, uint cchBufferLength);
    }
}