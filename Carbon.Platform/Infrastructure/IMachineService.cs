namespace Carbon.Platform
{
    using System.Threading.Tasks;

    public interface IMachineService
    {
        Task<MachineInfo> GetAsync();

        Task<int> RegisterAsync(MachineInfo machine);

        Task<MachineInfo> FindAsync(int id);

        Task SetStatusAsync(int id, DeviceStatus status);
    }
}