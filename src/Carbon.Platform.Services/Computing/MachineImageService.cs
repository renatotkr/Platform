using System;
using System.Threading.Tasks;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class MachineImageService : IMachineImageService
    {
        private readonly PlatformDb db;

        public MachineImageService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<MachineImageInfo> GetAsync(long id)
        {
            return await db.MachineImages.FindAsync(id).ConfigureAwait(false) 
                ?? throw ResourceError.NotFound(ResourceTypes.MachineImage, id);
        }

        public async Task<MachineImageInfo> GetAsync(ResourceProvider provider, string id)
        {
            var machineImage = await db.MachineImages.FindAsync(provider, id).ConfigureAwait(false);

            if (machineImage == null)
            {
                // Automatically register machine images until we can depend on the manager to register them before use

                var registerRequest = new RegisterMachineImageRequest(
                    name     : Guid.NewGuid().ToString().Replace("-", ""),
                    resource : new ManagedResource(provider, ResourceTypes.MachineImage, id),
                    type     : MachineImageType.Machine
                );

                machineImage = await RegisterAsync(registerRequest).ConfigureAwait(false);
            }

            return machineImage;
        }

        public async Task<MachineImageInfo> RegisterAsync(RegisterMachineImageRequest request)
        {
            #region Preconditions
            
            Validate.Object(request, nameof(request));

            #endregion

            var image = new MachineImageInfo(
                id       : db.MachineImages.Sequence.Next(),
                type     : request.Type,
                name     : request.Name,
                resource : request.Resource
            );

            await db.MachineImages.InsertAsync(image).ConfigureAwait(false);

            return image;
        }
    }
}