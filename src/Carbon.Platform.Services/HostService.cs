using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

using Carbon.Data;
using Carbon.Data.Expressions;

using ec2 = Amazon.Ec2;

using Dapper;

namespace Carbon.Platform.Services
{
    using Apps;
    using Computing;
    using Json;
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

        public async Task<HostInfo> GetAsync(long id)
        {
            var host = await db.Hosts.FindAsync(id).ConfigureAwait(false);

            // Throw if not found

            return host;
        }

        public async Task<HostInfo> GetAsync(ResourceProvider provider, string id)
        {
            var host = await db.Hosts.FindAsync(provider, id).ConfigureAwait(false);

            return host ?? await GetEc2Instance(id).ConfigureAwait(false);
        }

        public async Task<ImageInfo> GetImageAsync(ResourceProvider provider, string id)
        {
            var image = await db.Images.FindAsync(aws, id).ConfigureAwait(false);

            if (image == null)
            {
                image = GetEc2Image(id);

                image.Id = GetNextId<ImageInfo>();

                await db.Images.InsertAsync(image).ConfigureAwait(false);

            }

            return image;
        }

        public Task<IReadOnlyList<AppInstance>> GetAppInstancesAsync(long hostId)
        {
            return db.AppInstances.QueryAsync(Eq("hostId", hostId));
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

        private static HostStatus GetStatus(ec2::InstanceState state)
        {
            switch (state.Name)
            {
                case "pending"       : return HostStatus.Pending;
                case "running"       : return HostStatus.Running;
                case "shutting-down" : return HostStatus.Terminating;
                case "terminated"    : return HostStatus.Terminated;
                case "stopping"      : return HostStatus.Suspending;
                case "stopped"       : return HostStatus.Suspended;

                default: throw new Exception("unexpected state:" + state.Name);
            }
        }

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

            var host = new HostInfo {
                Id            = GetNextId<HostInfo>(),
                Type          = HostType.Virtual,
                Status        = GetStatus(instance.InstanceState),
                MachineTypeId = machineTypeId,
                Addresses     = new List<IPAddress>() {
                    IPAddress.Parse(instance.IpAddress),
                    IPAddress.Parse(instance.PrivateIpAddress)
                },
                Details    = new JsonObject { },
                ProviderId = aws.Id,
                ResourceId = instance.InstanceId,
                LocationId = location.Id,
                NetworkId  = network.Id,
                ImageId    = image?.Id ?? 0,
                Created    = instance.LaunchTime
            };

            // TODO: Lookup machineId

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

                networkInterface.NetworkId = network.Id;
                networkInterface.HostId = host.Id;

                await db.NetworkInterfaces.InsertAsync(networkInterface).ConfigureAwait(false);
            }

            return host;
        }


        private ImageInfo GetEc2Image(string id)
        {
            // TODO: Fetch th edetails
            return new ImageInfo {
                ProviderId = aws.Id,
                ResourceId = id
            };
        }

        private long GetNextId<T>()
        {
            var dataset = DatasetInfo.Get<T>();

            using (var connection = db.Context.GetConnection())
            {
                var id = connection.ExecuteScalar<long>($"SELECT `id` FROM `{dataset.Name}` ORDER BY `id` DESC LIMIT 1");

                return id + 1;
            }
        }

        private static async Task<long> GetNextScopedIdAsync<T>(Dataset<T, long> dataset, long scopeId)
        {
            var range = ScopedId.GetRange(scopeId);

            var count = await dataset.CountAsync(Between("id", range.Start, range.End)).ConfigureAwait(false);

            return ScopedId.Create(scopeId, count);
        }
    }

    #endregion
}