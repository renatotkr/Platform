using System;
using System.Threading.Tasks;

using Amazon.Ec2;

using Carbon.Platform.Computing;
using Carbon.Platform.Resources;
using Carbon.Platform.Storage;

namespace Carbon.Platform.Services
{
    public class VolumeManager
    {
        private readonly Ec2Client ec2;
        private readonly IVolumeService volumeService;

        public VolumeManager(IVolumeService volumes, Ec2Client ec2)
        {
            this.ec2     = ec2     ?? throw new ArgumentNullException(nameof(ec2));
            this.volumeService = volumes ?? throw new ArgumentNullException(nameof(volumes));
        }

        public async Task<VolumeInfo> GetAsync(
            ResourceProvider provider, 
            string resourceId,
            IHost host = null)
        {
            var volume = await volumeService.FindAsync(provider, resourceId);;
            
            // If the volume isn't register, register it now...

            if (volume == null)
            {
                var ec2Volume = await ec2.DescribeVolumeAsync(resourceId);;

                var location = Locations.Get(provider, ec2Volume.AvailabilityZone);

                var registerRequest = new RegisterVolumeRequest( 
                    size     : ByteSize.FromGiB(ec2Volume.Size),
                    ownerId  : 1,
                    resource : ManagedResource.Volume(location, ec2Volume.VolumeId)
                );

                volume = await volumeService.RegisterAsync(registerRequest);;
            }

            return volume;
        }
    }
}