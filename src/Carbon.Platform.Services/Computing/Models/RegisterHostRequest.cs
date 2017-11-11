﻿using System.ComponentModel.DataAnnotations;
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
            HostStatus status = HostStatus.Pending,
            HostType type = HostType.Virtual)
        {
            Validate.NotNull(cluster,  nameof(cluster));
            Validate.NotNull(location, nameof(location));
            Validate.NotNull(image,    nameof(image));

            Addresses = addresses;
            LocationId    = location.Id;
            EnvironmentId = cluster.EnvironmentId;
            ClusterId     = cluster.Id;
            ImageId       = image.Id;

            MachineType = machineType != null ? new MachineTypeDescriptor {
                Id = machineType.Id,
                Name = machineType.Name
            } : null;

            Status = status;
            Resource = resource;
            Type = type;
            OwnerId = ownerId;
        }

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

        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; set; }

        public RegisterVolumeRequest[] Volumes { get; set; }

        public RegisterNetworkInterfaceRequest[] NetworkInterfaces { get; set; }
    }

    public class MachineTypeDescriptor
    {
        public long? Id { get; set; }

        public string Name { get; set; }
    }
}