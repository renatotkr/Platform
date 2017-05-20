using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public interface IMachineImageService
    {
        Task<MachineImage> GetAsync(long id);

        Task<MachineImage> GetAsync(ResourceProvider provider, string id); // bool autoRegister = true

        Task<MachineImage> RegisterAsync(RegisterMachineImageRequest request);
    }
}