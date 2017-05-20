using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public interface IMachineImageService
    {
        Task<MachineImageInfo> GetAsync(long id);

        Task<MachineImageInfo> GetAsync(ResourceProvider provider, string id); // bool autoRegister = true

        Task<MachineImageInfo> RegisterAsync(RegisterMachineImageRequest request);
    }
}