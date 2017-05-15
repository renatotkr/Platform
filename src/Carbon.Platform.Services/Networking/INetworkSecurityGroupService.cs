using System.Threading.Tasks;

namespace Carbon.Platform.Networking
{
    public interface INetworkSecurityGroupService
    {
        Task<NetworkSecurityGroup> FindAsync(ResourceProvider provider, string resourceId);

        Task<NetworkSecurityGroup> GetAsync(long id);

        Task<NetworkSecurityGroup> RegisterAsync(RegisterNetworkSecurityGroupRequest request);
    }
}