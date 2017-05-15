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
        private readonly NetworkService networkService;
        private readonly NetworkInterfaceService networkInterfaces;
        private readonly ec2.Ec2Client ec2;

        public NetworkInterfaceManager(
            ec2.Ec2Client ec2, 
            NetworkService networkService,
            NetworkInterfaceService networkInterfaces)
        {
            this.networkService     = networkService;
            this.networkInterfaces = networkInterfaces ?? throw new ArgumentNullException(nameof(networkInterfaces));
            this.ec2               = ec2 ?? throw new ArgumentNullException(nameof(ec2));
        }
       
        public async Task<NetworkInterfaceInfo> GetAsync(ResourceProvider provider, string id)
        {
            var record = await networkInterfaces.FindAsync(provider, id).ConfigureAwait(false);

            if (record == null)
            {
                var nic = await ec2.DescribeNetworkInterfaceAsync(id).ConfigureAwait(false)
                    ?? throw new ResourceNotFoundException($"aws:networkInterface/{id}");

                var network = await networkService.GetAsync(Aws, nic.VpcId);

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
                    ? await networkService.FindSubnetAsync(Aws, nic.SubnetId).ConfigureAwait(false)
                    : null;

                // TODO: Lookup host & create an attachment... 

                //  ni.Attachment?.AttachTime ?? DateTime.UtcNow

                var registerRequest = new RegisterNetworkInterfaceRequest(
                    mac      : MacAddress.Parse(nic.MacAddress),
                    subnetId : subnet?.Id ?? 0,
                    resource : new ManagedResource(Aws, ResourceType.NetworkInterface, nic.NetworkInterfaceId)
                );
               

                record = await networkInterfaces.RegisterNetworkInterfaceAsync(registerRequest).ConfigureAwait(false);
            }

            return record;
        }
    }
}