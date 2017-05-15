using System;
using System.Threading.Tasks;

using Amazon.Ec2;

using Carbon.Platform.Computing;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Services
{
    public class VolumeManager
    {
        private readonly PlatformDb db;
        private readonly Ec2Client ec2;

        public VolumeManager(PlatformDb db, Ec2Client ec2)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.ec2 = ec2;
        }

        const int _1GiB = 1_073_741_824;

        public async Task<VolumeInfo> GetAsync(ResourceProvider provider, string name, IHost host = null)
        {
            var volume = await db.Volumes.FindAsync(provider, name).ConfigureAwait(false);

            if (volume == null)
            {
                var ec2Volume = await ec2.DescribeVolumeAsync(name).ConfigureAwait(false);

                var location = Locations.Get(provider, ec2Volume.AvailabilityZone);

                volume = new VolumeInfo(
                    id       : db.Volumes.Sequence.Next(),
                    size     : (long)ec2Volume.Size * _1GiB,
                    resource : ManagedResource.Volume(location, ec2Volume.VolumeId),
                    hostId   : host.Id
                );

                await db.Volumes.InsertAsync(volume).ConfigureAwait(false);
            }

            return volume;
        }
    }
}