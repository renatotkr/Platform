using System;
using System.Threading.Tasks;
using Carbon.Platform.Resources;
using Carbon.Platform.Services;

namespace Carbon.Platform.Storage
{
    public class VolumeService : IVolumeService
    {
        private readonly PlatformDb db;

        public VolumeService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<VolumeInfo> GetAsync(long id)
        {
            return await db.Volumes.FindAsync(id).ConfigureAwait(false)
                ?? throw ResourceError.NotFound(ResourceTypes.Volume, id);
        }

        public Task<VolumeInfo> GetAsync(string name)
        {
            if (long.TryParse(name, out var id)) return GetAsync(id);
            
            (var provider, var resourceId) = ResourceName.Parse(name);

            return FindAsync(provider, resourceId) 
                ?? throw ResourceError.NotFound(provider, ResourceTypes.Volume, name);
        }

        public Task<VolumeInfo> FindAsync(ResourceProvider provider, string id)
        {
            return db.Volumes.FindAsync(provider, id);
        }

        public async Task<VolumeInfo> RegisterAsync(RegisterVolumeRequest request)
        {
            var volume = new VolumeInfo(
                id       : db.Volumes.Sequence.Next(),
                size     : request.Size.TotalBytes,
                resource : request.Resource,
                hostId   : request.HostId
            );

            await db.Volumes.InsertAsync(volume).ConfigureAwait(false);

            return volume;
        }
    }
}