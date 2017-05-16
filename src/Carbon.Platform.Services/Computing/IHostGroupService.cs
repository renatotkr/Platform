using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public interface IHostGroupService
    {
        Task<HostGroup> CreateAsync(CreateHostGroupRequest request);

        Task<HostGroup> GetAsync(IEnvironment env, ILocation location);

        Task<HostGroup> GetAsync(long id);
    }
}