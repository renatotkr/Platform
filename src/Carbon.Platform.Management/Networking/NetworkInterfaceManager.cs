using System;
using System.Threading.Tasks;

using Amazon.Ec2;

using Carbon.Net;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    using static ResourceProvider;

    public class NetworkInterfaceManager
    {
        private readonly INetworkService networkService;
        private readonly INetworkInterfaceService networkInterfaces;
        private readonly ISubnetService subnetService;
        private readonly NetworkSecurityGroupManager nsgManager;
        private readonly Ec2Client ec2;

        public NetworkInterfaceManager(
            Ec2Client ec2, 
            INetworkService networkService,
            ISubnetService subnetService,
            INetworkInterfaceService networkInterfaceService,
            INetworkSecurityGroupService nsgService)
        {
            this.networkService    = networkService          ?? throw new ArgumentNullException(nameof(networkService));
            this.networkInterfaces = networkInterfaceService ?? throw new ArgumentNullException(nameof(networkInterfaceService));
            this.subnetService     = subnetService           ?? throw new ArgumentNullException(nameof(subnetService));
            this.ec2               = ec2                     ?? throw new ArgumentNullException(nameof(ec2));

            this.nsgManager        = new NetworkSecurityGroupManager(nsgService);
        }
       
        public async Task<NetworkInterfaceInfo> GetAsync(ResourceProvider provider, string resourceId)
        {
            var record = await networkInterfaces.FindAsync(provider, resourceId);;

            if (record == null)
            {
                var nic = await ec2.DescribeNetworkInterfaceAsync(resourceId)
                    ?? throw ResourceError.NotFound(Aws, ResourceTypes.NetworkInterface, resourceId);

                var network = await networkService.GetAsync(Aws, nic.VpcId);
                var region = Locations.Get(network.LocationId);

                SubnetInfo subnet = nic.SubnetId != null
                   ? await subnetService.FindAsync(Aws, nic.SubnetId)
                   : null;

                var securityGroupIds = new long[nic.Groups.Length];
                
                for (var i = 0;  i < securityGroupIds.Length; i++)
                {
                    var nsg = await nsgManager.GetAsync(network, nic.Groups[i]);;

                    securityGroupIds[i] = nsg.Id;
                }
                
                // TODO: Lookup host & create an attachment (ni.Attachment?.AttachTime) ... 

                var registerRequest = new RegisterNetworkInterfaceRequest(
                    mac              : MacAddress.Parse(nic.MacAddress),
                    subnetId         : subnet?.Id ?? 0,
                    securityGroupIds : securityGroupIds,
                    resource         : ManagedResource.NetworkInterface(region, nic.NetworkInterfaceId)
                );
                
                record = await networkInterfaces.RegisterAsync(registerRequest);;
            }

            return record;
        }
    }
}