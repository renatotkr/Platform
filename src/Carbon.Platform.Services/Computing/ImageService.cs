using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    using static Expression;

    public class ImageService : IImageService
    {
        private readonly PlatformDb db;

        public ImageService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<ImageInfo>> ListAsync()
        {
            return db.Images.QueryAsync(IsNull("deleted"));
        }

        public Task<IReadOnlyList<ImageInfo>> ListAsync(long ownerId)
        {
            return db.Images.QueryAsync(And(Eq("ownerId", ownerId), IsNull("deleted")));
        }

        public async Task<ImageInfo> GetAsync(long id)
        {
            return await db.Images.FindAsync(id) 
                ?? throw ResourceError.NotFound(ResourceTypes.Image, id);
        }

        public async Task<ImageInfo> GetAsync(long ownerId, string name)
        {
            return await db.Images.QueryFirstOrDefaultAsync(
                And(Eq("ownerId", ownerId), Eq("name", name))
             ) ?? throw ResourceError.NotFound(ResourceTypes.Image, ownerId, name);
        }

        public Task<bool> ExistsAsync(ResourceProvider provider, string resourceId)
        {
            return db.Images.ExistsAsync(
                And(Eq("providerId", provider.Id), Eq("resourceId", resourceId))
            );
        }

        public async Task<ImageInfo> FindAsync(ResourceProvider provider, string resourceId)
        {
            Ensure.NotNull(resourceId, nameof(resourceId));

            return await db.Images.QueryFirstOrDefaultAsync(
                And(Eq("providerId", provider.Id), Eq("resourceId", resourceId)
            ));
        }

        public async Task<ImageInfo> GetAsync(ResourceProvider provider, string resourceId)
        {
            var image = await FindAsync(provider, resourceId);

            if (image == null)
            {
                // Automatically register machine images until we can depend on the manager to register them for us before use

                var registerRequest = new RegisterImageRequest(
                    name     : Guid.NewGuid().ToString("N"),
                    ownerId  : ResourceProvider.Aws.Id, // assume to be AWS for now
                    resource : new ManagedResource(provider, ResourceTypes.Image, resourceId),
                    type     : ImageType.Machine
                );

                image = await RegisterAsync(registerRequest);
            }

            return image;
        }

        public async Task<ImageInfo> RegisterAsync(RegisterImageRequest request)
        {
            Ensure.NotNull(request, nameof(request));

            var image = new ImageInfo(
                id         : await db.Images.Sequence.NextAsync(),
                type       : request.Type,
                name       : request.Name,
                ownerId    : request.OwnerId,
                size       : request.Size,
                resource   : request.Resource,
                properties : request.Properties
            );

            await db.Images.InsertAsync(image);

            return image;
        }
        
        public async Task<bool> DeleteAsync(IImage record)
        {
            Ensure.NotNull(record, nameof(record));

            return await db.Images.PatchAsync(record.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}