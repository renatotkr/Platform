using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public interface IClusterService
    {
        Task<Cluster> CreateAsync(CreateClusterRequest request);

        Task<Cluster> GetAsync(IEnvironment env, ILocation location);

        Task<Cluster> GetAsync(long id);
    }
}