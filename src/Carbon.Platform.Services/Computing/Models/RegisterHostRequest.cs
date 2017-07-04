using System.ComponentModel.DataAnnotations;
using Carbon.Platform.Networking;
using Carbon.Platform.Resources;
using Carbon.Platform.Storage;

namespace Carbon.Platform.Computing
{
    public class RegisterHostRequest
    {
        public RegisterHostRequest() { }

        public RegisterHostRequest(
            string[] addresses,
            ILocation location,
            ICluster cluster,
            IImage image,
            IProgram program,
            IMachineType machineType,
            long ownerId,
            ManagedResource resource,
            long networkId = 0,
            HostStatus status = HostStatus.Pending,
            HostType type = HostType.Virtual)
        {
            LocationId    = location.Id;
            Addresses     = addresses;
            EnvironmentId = cluster.EnvironmentId;
            ClusterId     = cluster.Id;
            ProgramId     = program?.Id;
            ImageId       = image.Id;
            MachineTypeId = machineType.Id;
            NetworkId     = networkId;
            Status        = status;
            Resource      = resource;
            Type          = type;
            OwnerId       = ownerId;
        }

        public ManagedResource Resource { get; set; }

        public HostType Type { get; set; }

        public HostStatus Status { get; set; }

        // private ip first...
        public string[] Addresses { get; set; }

        public long EnvironmentId { get; set; }

        public int LocationId { get; set; }

        public long MachineTypeId { get; set; }

        public long ImageId { get; set; }

        public long NetworkId { get; set; }

        public long ClusterId { get; set; }

        public long? ProgramId { get; set; }

        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; set; }

        public RegisterVolumeRequest[] Volumes { get; set; }

        public RegisterNetworkInterfaceRequest[] NetworkInterfaces { get; set; }
    }
}