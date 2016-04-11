using System.Threading.Tasks;

namespace Carbon.Platform
{
    public interface IMachineService
    {
        Task<MachineInfo> GetAsync();

        Task<int> RegisterAsync(MachineInfo machine);

        Task<MachineInfo> FindAsync(int id);
    }
}