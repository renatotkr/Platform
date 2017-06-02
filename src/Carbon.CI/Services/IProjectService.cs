using System.Threading.Tasks;

namespace Carbon.CI
{
    public interface IProjectService
    {
        Task<Project> CreateAsync(CreateProjectRequest request);

        Task<Project> GetAsync(long id);

        Task<Project> GetAsync(long ownerId, string name);
    }
}