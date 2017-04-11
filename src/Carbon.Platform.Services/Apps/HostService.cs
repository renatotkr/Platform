using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

using Carbon.Data.Expressions;

using ec2 = Amazon.Ec2;

namespace Carbon.Platform.Services
{
    using Computing;
    using Storage;
    using Networking;

    using static Expression;

    public class HostService
    {
        private readonly PlatformDb db;
        private readonly ec2::Ec2Client ec2;
        private readonly VolumeService volumeService;
        private readonly NetworkService networkService;

        public HostService(PlatformDb db, ec2::Ec2Client ec2)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.ec2 = ec2;
            this.volumeService = new VolumeService(db, ec2);
            this.networkService = new NetworkService(db, ec2);
        }

        // Cache host?

        public async Task<HostInfo> GetAsync(long id)
        {
            return await db.Hosts.FindAsync(id).ConfigureAwait(false) ?? throw new Exception($"host#{id} does not exist");
        }

        public async Task<HostInfo> GetAsync(ResourceProvider provider, string id)
        {
            var host = await db.Hosts.FindAsync(provider, id).ConfigureAwait(false);

            return host ?? await GetEc2Instance(id).ConfigureAwait(false);
        }

        public async Task<MachineImageInfo> GetImageAsync(ResourceProvider provider, string id)
        {
            var image = await db.MachineImages.FindAsync(aws, id).ConfigureAwait(false);

            if (image == null)
            {
                image = GetEc2Image(id);


                await db.MachineImages.InsertAsync(image).ConfigureAwait(false);
            }

            return image;
        }

        public Task<IReadOnlyList<NetworkInterfaceInfo>> GetNetworkInterfacesAsync(long hostId)
        {
            return db.NetworkInterfaces.QueryAsync(Eq("hostId", hostId));
        }

        public Task<IReadOnlyList<VolumeInfo>> GetVolumesAsync(long hostId)
        {
            return db.Volumes.QueryAsync(Eq("hostId", hostId));
        }

        #region EC2 Helpers

        private static readonly ResourceProvider aws = ResourceProvider.Amazon;

        private async Task<HostInfo> GetEc2Instance(string id)
        {
            var instance = await ec2.DescribeInstanceAsync(id).ConfigureAwait(false);

            if (instance == null)
                throw new Exception($"Instance {id} not found");

            if (instance.VpcId == null)
                throw new Exception("Ec2 instance MUST be inside a VPC");

            var network = await networkService.GetNetworkAsync(aws, instance.VpcId).ConfigureAwait(false);

            // "imageId": "ami-1647537c",

            var image = instance.ImageId != null ? await GetImageAsync(aws, instance.ImageId).ConfigureAwait(false) : null;

            var location = Locations.Get(aws, instance.Placement.AvailabilityZone);

            long machineTypeId = 0;

            try
            {
                machineTypeId = AwsInstanceType.GetId(instance.InstanceType);
            }
            catch
            { }

            var env = new AppEnvironment(0, 0, "unknown");

            var resource = ManagedResource.Host(location, instance.InstanceId);

            var addresses = new[] {
                IPAddress.Parse(instance.PrivateIpAddress),
                IPAddress.Parse(instance.IpAddress)
            };

            var host = new HostInfo(
                id        : db.Context.GetNextId<HostInfo>(),
                type      : HostType.Virtual,
                status    : instance.InstanceState.ToStatus(),
                addresses : addresses,
                resource  : resource,
                env       : env,
                created   : instance.LaunchTime) {
                MachineTypeId = machineTypeId,
                NetworkId = network.Id,
                MachineImageId   = image?.Id ?? 0
            };

            await db.Hosts.InsertAsync(host).ConfigureAwait(false);

            foreach (var v in instance.BlockDeviceMappings)
            {
                if (v.Ebs == null) continue;

                var volume = await volumeService.GetAsync(aws, v.Ebs.VolumeId).ConfigureAwait(false);
            }

            foreach (var ec2NetworkInterface in instance.NetworkInterfaces)
            {
                if (ec2NetworkInterface.VpcId != instance.VpcId)
                {
                    throw new Exception("Host's network interface belongs to different VPC:" + ec2NetworkInterface.VpcId);
                }

                var networkInterface = await networkService.ConfigureEc2NetworkInterfaceAsync(ec2NetworkInterface).ConfigureAwait(false);

                networkInterface.HostId = host.Id;

                await db.NetworkInterfaces.InsertAsync(networkInterface).ConfigureAwait(false);
            }
        
            return host;
        }

        private MachineImageInfo GetEc2Image(string id)
        {
            // TODO: Fetch the image details

            return new MachineImageInfo(
                id          : db.Context.GetNextId<MachineImageInfo>(),
                type        : MachineImageType.Machine,
                name        : Guid.NewGuid().ToString(),
                description : null,
                resource    : new ManagedResource(aws, ResourceType.MachineImage, id)
            );
        }
    }

    #endregion
}