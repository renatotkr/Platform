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
            Cluster cluster,
            ILocation location,
            IMachineImage machineImage,
            IMachineType machineType,
            long ownerId,
            ManagedResource resource,
            long networkId = 0,
            HostStatus status = HostStatus.Pending)
        {
            Addresses      = addresses;
            EnvironmentId  = cluster.EnvironmentId.Value;
            Location       = location;
            MachineImageId = machineImage.Id;
            MachineTypeId  = machineType.Id;
            NetworkId      = networkId;
            Status         = status;
            Resource       = resource;
            ClusterId      = cluster.Id;
            OwnerId        = ownerId;
        }

        // private ip first...
        public string[] Addresses { get; set; }

        public long EnvironmentId { get; set; }

        public ILocation Location { get; set; }

        public HostStatus Status { get; set; }

        public ManagedResource Resource { get; set; }

        public long MachineTypeId { get; set; }

        public long MachineImageId { get; set; }

        public long NetworkId { get; set; }

        public long ClusterId { get; set; }

        public long OwnerId { get; set; }

        public RegisterVolumeRequest[] Volumes { get; set; }

        public RegisterNetworkInterfaceRequest[] NetworkInterfaces { get; set; }
    }
}