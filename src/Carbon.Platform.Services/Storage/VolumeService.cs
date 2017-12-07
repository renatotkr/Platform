using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Computing;
using Carbon.Platform.Resources;
using Carbon.Platform.Services;

namespace Carbon.Platform.Storage
{
    using static Expression;

    public class VolumeService : IVolumeService
    {
        private readonly PlatformDb db;

        public VolumeService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<VolumeInfo> GetAsync(long id)
        {
            return await db.Volumes.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.Volume, id);
        }

        public async Task<VolumeInfo> GetAsync(string name)
        {
            Validate.NotNullOrEmpty(name, nameof(name));

            if (long.TryParse(name, out var id))
            {
                return await GetAsync(id);
            }

            var (provider, resourceId) = ResourceName.Parse(name);

            return await FindAsync(provider, resourceId) 
                ?? throw ResourceError.NotFound(ManagedResource.Volume(provider, name));
        }

        public Task<VolumeInfo> FindAsync(ResourceProvider provider, string id)
        {
            return db.Volumes.FindAsync(provider, id);
        }

        public async Task<VolumeInfo> RegisterAsync(RegisterVolumeRequest request)
        {
            Validate.Object(request, nameof(request)); // Validate the request

            var volume = new VolumeInfo(
                id       : await db.Volumes.Sequence.NextAsync(),
                size     : request.Size.TotalBytes,
                ownerId  : request.OwnerId,
                resource : request.Resource,
                hostId   : request.HostId
            );

            await db.Volumes.InsertAsync(volume);

            return volume;
        }

        public Task<IReadOnlyList<VolumeInfo>> ListAsync(IHost host)
        {
            Validate.NotNull(host, nameof(host));

            return db.Volumes.QueryAsync(Eq("hostId", host.Id));
        }

        public Task<IReadOnlyList<VolumeInfo>> ListAsync(long ownerId)
        {
            return db.Volumes.QueryAsync(
                And(Eq("ownerId", ownerId), IsNotNull("deleted"))
            );
        }

        public async Task<bool> DeleteAsync(IVolume volume)
        {
            Validate.NotNull(volume, nameof(volume));

            return await db.Volumes.PatchAsync(volume.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}