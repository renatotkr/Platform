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
            long ownerId,
            string[] addresses,
            ILocation location,
            ICluster cluster,
            IImage image,
            IProgram program,
            IMachineType machineType,
            ManagedResource resource,
            HostStatus status = HostStatus.Pending,
            HostType type = HostType.Virtual)
        {
            Validate.Id(ownerId,          nameof(ownerId));
            Validate.NotNull(cluster,     nameof(cluster));
            Validate.NotNull(location,    nameof(location));
            Validate.NotNull(image,       nameof(image));
            Validate.NotNull(machineType, nameof(machineType));

            OwnerId       = ownerId;
            Addresses     = addresses;
            LocationId    = location.Id;
            EnvironmentId = cluster.EnvironmentId;
            ClusterId     = cluster.Id;
            ImageId       = image.Id;

            MachineType = new MachineTypeDescriptor {
                Id   = machineType.Id,
                Name = machineType.Name
            };

            Status = status;
            Resource = resource;
            Type = type;
        }

        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; set; }

        public ManagedResource Resource { get; set; }

        public HostType Type { get; set; }

        public HostStatus Status { get; set; }

        // private ip first...
        public string[] Addresses { get; set; }

        public long EnvironmentId { get; set; }

        public int LocationId { get; set; }
        
        public MachineTypeDescriptor MachineType { get; set; }

        public long ImageId { get; set; }

        public long ClusterId { get; set; }

        public RegisterVolumeRequest[] Volumes { get; set; }

        public RegisterNetworkInterfaceRequest[] NetworkInterfaces { get; set; }
    }

    public struct MachineTypeDescriptor
    {
        public long? Id { get; set; }

        public string Name { get; set; }
    }
}