using System.Threading.Tasks;

namespace Carbon.Platform
{
    public interface IEnvironmentService
    {
        Task<EnvironmentInfo> GetAsync(long id);

        Task<EnvironmentInfo> GetAsync(long programId, EnvironmentType type);
    }
}