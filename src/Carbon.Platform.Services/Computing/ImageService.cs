using System;
using System.Threading.Tasks;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class ImageService : IImageService
    {
        private readonly PlatformDb db;

        public ImageService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Image> GetAsync(long id)
        {
            return await db.Images.FindAsync(id).ConfigureAwait(false) 
                ?? throw ResourceError.NotFound(ResourceTypes.Image, id);
        }

        public async Task<Image> GetAsync(ResourceProvider provider, string resourceId)
        {
            var image = await db.Images.FindAsync(provider, resourceId).ConfigureAwait(false);

            if (image == null)
            {
                // Automatically register machine images until we can depend on the manager to register them before use

                var registerRequest = new RegisterImageRequest(
                    name     : Guid.NewGuid().ToString("N"),
                    resource : new ManagedResource(provider, ResourceTypes.Image, resourceId),
                    type     : ImageType.Machine
                );

                image = await RegisterAsync(registerRequest).ConfigureAwait(false);
            }

            return image;
        }

        public async Task<Image> RegisterAsync(RegisterImageRequest request)
        {
            #region Preconditions
            
            Validate.Object(request, nameof(request));

            #endregion

            var image = new Image(
                id       : await db.Images.Sequence.NextAsync(),
                type     : request.Type,
                name     : request.Name,
                size     : request.Size,
                ownerId  : request.OwnerId,
                resource : request.Resource
            );

            await db.Images.InsertAsync(image).ConfigureAwait(false);

            return image;
        }
    }
}