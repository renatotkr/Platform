using System.Threading.Tasks;

using Carbon.Platform.Computing;

namespace Carbon.Platform.Management
{
    public interface IClusterManager
    {
        Task DeregisterHostAsync(Cluster cluster, HostInfo host);

        Task RegisterHostAsync(Cluster cluster, HostInfo host);

        Task UpdateImageAsync(Cluster cluster, ImageInfo newImage);
    }
}