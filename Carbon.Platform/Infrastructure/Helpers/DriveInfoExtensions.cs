using System.IO;

namespace Carbon.Platform
{
    public static class DriveInfoExtensions
    {
        public static VolumeInfo ToVolumeInfo(this DriveInfo drive, MachineInfo machine)
        {
           return new VolumeInfo {
                MachineId = machine.Id,
                Name = drive.Name.Substring(0, 1),
                Available = drive.AvailableFreeSpace,
                Size = drive.TotalSize,
                Used = drive.TotalSize - drive.TotalFreeSpace,
                Drive = drive
           };
        }
    }
}