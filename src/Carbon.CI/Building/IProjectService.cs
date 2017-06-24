using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.CI
{
    public interface IProjectService
    {
        Task<IReadOnlyList<ProjectInfo>> ListAsync(long ownerId);

        Task<ProjectInfo> CreateAsync(CreateProjectRequest request);

        Task<ProjectInfo> GetAsync(long id);

        Task<ProjectInfo> GetAsync(long ownerId, string name);
    }
}