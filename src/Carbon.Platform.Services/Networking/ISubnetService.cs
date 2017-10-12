using System.Threading.Tasks;

namespace Carbon.Platform.Networking
{
    public interface ISubnetService
    {
        Task<SubnetInfo> FindAsync(ResourceProvider provider, string resourceId);

        Task<SubnetInfo> GetAsync(long id);

        Task<SubnetInfo> GetAsync(ResourceProvider provider, string resourceId);

        Task<SubnetInfo> GetAsync(string name);

        Task<SubnetInfo> RegisterAsync(RegisterSubnetRequest request);

        Task<bool> DeleteAsync(ISubnet subnet);
    }
}