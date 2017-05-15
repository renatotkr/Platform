using System;
using System.Net;
using System.Threading.Tasks;

using Carbon.Net;
using Carbon.Platform.Resources;

using ec2 = Amazon.Ec2;

namespace Carbon.Platform.Networking
{
    public class NetworkInterfaceManager
    {
        private readonly PlatformDb db;
        private readonly ec2.Ec2Client ec2;

        public NetworkInterfaceManager(ec2.Ec2Client ec2, PlatformDb db)
        {
            this.db  = db  ?? throw new ArgumentNullException(nameof(db));
            this.ec2 = ec2 ?? throw new ArgumentNullException(nameof(ec2));
        }
       
        public async Task<NetworkInterfaceInfo> GetAsync(ResourceProvider provider, string id)
        {
            var record = await db.NetworkInterfaces.FindAsync(provider, id).ConfigureAwait(false);

            if (record == null)
            {
                var ec2Nic = await ec2.DescribeNetworkInterfaceAsync(id).ConfigureAwait(false)
                    ?? throw new Exception($"ec2:networkInterface/{id} not found");

                var host = await db.Hosts.FindAsync(provider, ec2Nic.Attachment.InstanceId).ConfigureAwait(false);

                var network = await db.Networks.FindAsync(ResourceProvider.Aws, ec2Nic.VpcId)
                    ?? throw new Exception($"ec2:network/{ec2Nic.VpcId} not found");

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

                var nic = await ConfigureAsync(ec2Nic, host?.Id).ConfigureAwait(false);
                
                await db.NetworkInterfaces.InsertAsync(nic).ConfigureAwait(false);
            }

            return record;
        }
        
        private async Task<NetworkInterfaceInfo> ConfigureAsync(ec2.NetworkInterface nic, long? hostId)
        {
            #region Preconditions

            if (nic == null)
                throw new ArgumentNullException(nameof(nic));

            #endregion

            SubnetInfo subnet = nic.SubnetId != null
                ? await db.Subnets.FindAsync(ResourceProvider.Aws, nic.SubnetId).ConfigureAwait(false) 
                : null;

            // TODO: Create an attachment...

            //  ni.Attachment?.AttachTime ?? DateTime.UtcNow

            return new NetworkInterfaceInfo(
                id        : db.NetworkInterfaces.Sequence.Next(),
                mac       : MacAddress.Parse(nic.MacAddress),
                addresses : Array.Empty<IPAddress>(), // todo
                subnetId  : subnet?.Id ?? 0,
                resource  : new ManagedResource(ResourceProvider.Aws, ResourceType.NetworkInterface, nic.NetworkInterfaceId)
            )
            {  HostId = hostId };

            // TODO: Create an attachment...

            //  ni.Attachment?.AttachTime ?? DateTime.UtcNow
        }
    }
}