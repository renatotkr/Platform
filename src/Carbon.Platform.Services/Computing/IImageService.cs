using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public interface IImageService
    {
        Task<IReadOnlyList<ImageInfo>> ListAsync();

        Task<ImageInfo> GetAsync(long id);

        Task<ImageInfo> GetAsync(ResourceProvider provider, string resourceId); // bool autoRegister = true

        Task<ImageInfo> RegisterAsync(RegisterImageRequest request);
    }
}