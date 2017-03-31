using System;
using System.Threading.Tasks;
using Carbon.Data;

using Amazon.Ec2;

using Dapper;

namespace Carbon.Platform.Services
{
    using Computing;
    using Storage;

    public class VolumeService
    {
        private readonly PlatformDb db;
        private readonly Ec2Client ec2;

        public VolumeService(PlatformDb db, Ec2Client ec2)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.ec2 = ec2;
        }

        const int _1GB = 1_073_741_824;

        public async Task<VolumeInfo> GetAsync(ResourceProvider provider, string name, HostInfo host = null)
        {
            var volume = await db.Volumes.FindAsync(provider, name).ConfigureAwait(false);

            if (volume == null)
            {
                var ec2Volume = await ec2.DescribeVolumeAsync(name).ConfigureAwait(false);

                var location = Locations.Get(provider, ec2Volume.AvailabilityZone);

                 volume = new VolumeInfo {
                    Id         = GetNextId<VolumeInfo>(),
                    Status     = VolumeStatus.Online,
                    ProviderId = provider.Id,
                    ResourceId = ec2Volume.VolumeId,
                    LocationId = location.Id,
                    HostId     = host?.Id,
                    Size       = (long)ec2Volume.Size * _1GB
                };

                await db.Volumes.InsertAsync(volume).ConfigureAwait(false);
            }

            return volume;
        }
        
        private long GetNextId<T>()
        {
            var dataset = DatasetInfo.Get<T>();

            using (var connection = db.Context.GetConnection())
            {
                var id = connection.ExecuteScalar<long>($"SELECT `id` FROM `{dataset.Name}` ORDER BY `id` DESC  LIMIT 1");

                return id + 1;
            }
        }
    }
}