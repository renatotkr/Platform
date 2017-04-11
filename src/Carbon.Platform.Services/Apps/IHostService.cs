using System.Threading.Tasks;

using Carbon.Platform.Computing;

namespace Carbon.Platform.Services
{
    public interface IHostService
    {
        Task<HostInfo> GetAsync(long id);
    }
}