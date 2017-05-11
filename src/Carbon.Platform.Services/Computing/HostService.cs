using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

using Amazon.Metadata;

using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

using Dapper;

namespace Carbon.Platform.Services
{
    using Computing;
    using Networking;

    using static Expression;

    public class HostService : IHostService
    {
        private readonly PlatformDb db;

        public HostService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<HostInfo> GetAsync(long id)
        {
            return await db.Hosts.FindAsync(id).ConfigureAwait(false) ?? throw new Exception($"host#{id} does not exist");
        }

        public async Task<HostInfo> FindAsync(ResourceProvider provider, string id)
        {
            return await db.Hosts.FindAsync(provider, id).ConfigureAwait(false);
        }

        public async Task<HostInfo> CreateAsync(InstanceIdentity instance, IEnvironment env)
        {
            #region Preconditions

            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            #endregion

            var location = Locations.Get(aws, instance.AvailabilityZone);

            long machineTypeId = 0;
            MachineImageInfo machineImage = null;

            try
            {
                machineTypeId = AwsInstanceType.GetId(instance.InstanceType);

                machineImage = await GetImageAsync(ResourceProvider.Amazon, instance.ImageId).ConfigureAwait(false);
            }
            catch
            { }

            var host = new HostInfo(
                id        : await GetNextId(location).ConfigureAwait(false),
                type      : HostType.Virtual,
                status    : HostStatus.Running,
                addresses : new[] { IPAddress.Parse(instance.PrivateIp) },
                resource  : ManagedResource.Host(location, instance.InstanceId),
                env       : env,
                networkId : 0,
                created   : DateTime.UtcNow
            ) {
                MachineTypeId  = machineTypeId,
                MachineImageId = machineImage?.Id ?? 0
            };


            await db.Hosts.InsertAsync(host).ConfigureAwait(false);

            return host;
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

        private MachineImageInfo GetEc2Image(string id)
        {
            // TODO: Fetch the image details

            return new MachineImageInfo (
                id       : db.MachineImages.IdGenerator.Next(),
                type     : MachineImageType.Machine,
                name     : Guid.NewGuid().ToString(),
                resource : new ManagedResource(aws, ResourceType.MachineImage, id)
            );
        }

        public Task<IReadOnlyList<NetworkInterfaceInfo>> GetNetworkInterfacesAsync(long hostId)
        {
            return db.NetworkInterfaces.QueryAsync(Eq("hostId", hostId));
        }

        public Task<IReadOnlyList<VolumeInfo>> GetVolumesAsync(long hostId)
        {
            return db.Volumes.QueryAsync(Eq("hostId", hostId));
        }
        
        // 4B per zone per region
        private async Task<HostId> GetNextId(ILocation location)
        {
            // Ensure the location exists
            if (await db.Locations.FindAsync(location.Id).ConfigureAwait(false) == null)
            {
                await db.Locations.InsertAsync(new LocationInfo(location.Id, location.Name)).ConfigureAwait(false);
            }

            int sequenceNumber;

            using (var connection = db.Context.GetConnection())
            {
                sequenceNumber = await connection.ExecuteScalarAsync<int>(
                    @"SELECT `hostCount` FROM `Locations` WHERE id = @id FOR UPDATE;
                    UPDATE `Locations`
                    SET `hostCount` = `hostCount` + 1
                    WHERE id = @id", location).ConfigureAwait(false) + 1;
            }

            return HostId.Create(location, sequenceNumber);
        }

        #region EC2 Helpers

        private static readonly ResourceProvider aws = ResourceProvider.Amazon;

        /*
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

            var env = new EnvironmentInfo(id: 1, appId: 1, type: EnvironmentType.Production);

            var resource = ManagedResource.Host(location, instance.InstanceId);

            var addresses = new[] {
                IPAddress.Parse(instance.PrivateIpAddress),
                IPAddress.Parse(instance.IpAddress)
            };
            
            var host = new HostInfo(
                id        : await GetNextId(location).ConfigureAwait(false),
                type      : HostType.Virtual,
                status    : instance.InstanceState.ToStatus(),
                addresses : addresses,
                resource  : resource,
                env       : env,
                networkId : network.Id,
                created   : instance.LaunchTime) {
                MachineTypeId  = machineTypeId,
                MachineImageId = image?.Id ?? 0
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
        

       
        */
    }

    #endregion
}