using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public interface IImageService
    {
        Task<Image> GetAsync(long id);

        Task<Image> GetAsync(ResourceProvider provider, string id); // bool autoRegister = true

        Task<Image> RegisterAsync(RegisterImageRequest request);
    }
}