using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

using ec2 = Amazon.Ec2;

namespace Carbon.Platform.Services
{
    using Apps;
    using Computing;
    using Json;
    using Storage;
    using Networking;

    using static Data.Expressions.Expression;

    public class HostService
    {
        private readonly PlatformDb db;
        private readonly ec2::Ec2Client ec2;

        public HostService(PlatformDb db, ec2::Ec2Client ec2)
        {
            #region Preconditions

            if (db == null) throw new ArgumentNullException(nameof(db));

            #endregion

            this.db = db;
            this.ec2 = ec2;
        }

        // Update Hearbeat

        public async Task<Host> GetAsync(long id)
        {
            var host = await db.Hosts.FindAsync(id).ConfigureAwait(false);

            if (host == null) return null;

            // Fetch apps?

            return host;
        }

        public Task<Host> GetAsync(PlatformProviderId provider, string id)
            => GetEc2Instance(id);

        public Task<IList<AppInstance>> GetAppInstancesAsync(long hostId)
            => db.AppInstances.QueryAsync(Eq("hostId", hostId));
        
        public Task<IList<NetworkInterfaceInfo>> GetNetworkInterfacesAsync(long hostId)
            => db.NetworkInterfaces.QueryAsync(Eq("hostId", hostId));

        public Task<IList<VolumeInfo>> GetVolumesAsync(long hostId)
            => db.Volumes.QueryAsync(Eq("hostId", hostId));

        #region Ec2 Helpers

        private InstanceStatus GetStatus(ec2::InstanceState state)
        {
            switch (state.Name)
            {
                case "pending"       : return InstanceStatus.Pending;
                case "running"       : return InstanceStatus.Running;
                case "shutting-down" : return InstanceStatus.Terminating;
                case "terminated"    : return InstanceStatus.Terminated;
                case "stopping"      : return InstanceStatus.Suspending;
                case "stopped"       : return InstanceStatus.Suspended;

                default: throw new Exception("unexpected state:" + state.Name);
            }
        }

        private async Task<Host> GetEc2Instance(string id)
        {
            var instance = await ec2.DescribeInstanceAsync(id).ConfigureAwait(false);

            var host = new Host {
                Type = HostType.VirtualInstance,
                RefId = instance.InstanceId,
                Addresses = new List<IPAddress>() {
                    IPAddress.Parse(instance.IpAddress),
                    IPAddress.Parse(instance.PrivateIpAddress)
                },
                Details = new JsonObject {
                    { "availabilityZone", instance.Placement.AvailabilityZone },
                    { "instanceType", instance.InstanceType },
                    { "hypervisor", instance.Hypervisor },
                    { "vpcId", instance.VpcId }
                },
                Provider = PlatformProviderId.Amazon,
                Status = GetStatus(instance.InstanceState),
                Modified = DateTime.UtcNow,
                Created = instance.LaunchTime
            };

            // "imageId": "ami-1647537c",

            if (instance.ImageId != null)
            {
                // Images may be shared
                var image = (await db.Images.QueryFirstOrDefaultAsync(Eq("refId", instance.ImageId)));

                if (image == null)
                {
                    image = new Image
                    {
                        RefId = instance.ImageId,
                        Architecture = instance.Architecture,
                        Created = DateTime.UtcNow
                    };

                    await db.Images.InsertAsync(image).ConfigureAwait(false);

                    host.ImageId = image.Id;
                }
            }

            await db.Hosts.InsertAsync(host);

            foreach (var v in instance.BlockDeviceMappings)
            {
                if (v.Ebs != null)
                {
                    var vv = await ec2.DescribeVolumeAsync(v.Ebs.VolumeId).ConfigureAwait(false);

                    var vol = new VolumeInfo {
                        RefId = v.Ebs.VolumeId,
                        Size = (long)vv.Size * 1073741824,
                        HostId = host.Id,
                        Created = vv.CreateTime
                    };

                    await db.Volumes.InsertAsync(vol).ConfigureAwait(false);
                }
            }

            foreach (var ni in instance.NetworkInterfaces)
            {
                var networkInterface = new NetworkInterfaceInfo {
                    Mac = ni.MacAddress,
                    RefId = ni.NetworkInterfaceId,
                    HostId = host.Id,
                    Created = DateTime.UtcNow
                };

                // ? IP addresses
                // ? Security groups

                await db.NetworkInterfaces.InsertAsync(networkInterface).ConfigureAwait(false);
            }

            return host;
        }
    }

    #endregion
}
