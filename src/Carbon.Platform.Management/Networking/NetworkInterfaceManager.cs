using System;
using System.Threading.Tasks;

using Carbon.Net;
using Carbon.Platform.Resources;

using ec2 = Amazon.Ec2;

namespace Carbon.Platform.Networking
{
    using static ResourceProvider;

    public class NetworkInterfaceManager
    {
        private readonly INetworkService networks;
        private readonly INetworkInterfaceService networkInterfaces;
        private readonly ISubnetService subnetService;
        private readonly ec2.Ec2Client ec2;

        public NetworkInterfaceManager(
            ec2.Ec2Client ec2, 
            INetworkService networkService,
            ISubnetService subnetService,
            INetworkInterfaceService networkInterfaces)
        {
            this.networks          = networkService    ?? throw new ArgumentNullException(nameof(networkService));
            this.networkInterfaces = networkInterfaces ?? throw new ArgumentNullException(nameof(networkInterfaces));
            this.subnetService     = subnetService     ?? throw new ArgumentNullException(nameof(subnetService));
            this.ec2               = ec2               ?? throw new ArgumentNullException(nameof(ec2));
        }
       
        public async Task<NetworkInterfaceInfo> GetAsync(ResourceProvider provider, string id)
        {
            var record = await networkInterfaces.FindAsync(provider, id).ConfigureAwait(false);

            if (record == null)
            {
                var nic = await ec2.DescribeNetworkInterfaceAsync(id).ConfigureAwait(false)
                    ?? throw new ResourceNotFoundException($"aws:networkInterface/{id}");

                var network = await networks.GetAsync(Aws, nic.VpcId);

                /*
                // Ensure all the security groups exist
                if (n.Groups != null)
                {
                    foreach (var group in n.Groups)
                    {
                        await GetNetworkSecurityGroupAsync(group, network);                       
                    }
                }
                */

                SubnetInfo subnet = nic.SubnetId != null
                    ? await subnetService.FindAsync(Aws, nic.SubnetId).ConfigureAwait(false)
                    : null;

                // TODO: Lookup host & create an attachment (ni.Attachment?.AttachTime) ... 
                

                var registerRequest = new RegisterNetworkInterfaceRequest(
                    mac      : MacAddress.Parse(nic.MacAddress),
                    subnetId : subnet?.Id ?? 0,
                    resource : new ManagedResource(Aws, ResourceTypes.NetworkInterface, nic.NetworkInterfaceId)
                );
               
                record = await networkInterfaces.RegisterAsync(registerRequest).ConfigureAwait(false);
            }

            return record;
        }
    }
}