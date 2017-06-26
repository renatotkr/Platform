using System.Threading.Tasks;

namespace Carbon.Platform.Environments
{
    public interface IEnvironmentService
    {
        Task<EnvironmentInfo> GetAsync(long id);

        Task<EnvironmentInfo> CreateAsync(CreateEnvironmentRequest request);
    }
}