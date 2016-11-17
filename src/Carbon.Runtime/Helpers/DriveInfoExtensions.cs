using System.IO;

namespace Carbon.Storage
{
    using Computing;

    public static class DriveInfoExtensions
    {
        public static VolumeInfo ToVolumeInfo(this DriveInfo drive)
            => new VolumeInfo {
                DeviceName = drive.Name.Substring(0, 1),
                Size       = drive.TotalSize,
                // FreeSpace  = drive.TotalFreeSpace
           };
    }
}

// TotalFreeSpace: This property indicates the total amount of free space available on the drive, not just what is available to the current user.
// AvailableFreeSpace: This property indicates the amount of free space available on the drive (taking account disk quotas into account)
