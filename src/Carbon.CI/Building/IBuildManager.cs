using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.CI
{
    public interface IBuildManager
    {
        Task<Build> GetAsync(long id);

        Task<Build> GetLatestAsync(long projectId);

        Task<Build> SyncAsync(Build build);

        Task<IReadOnlyList<Build>> ListAsync(long projectId, int take = 1000);

        Task<Build> StartAsync(StartBuildRequest request);

        Task UpdateAsync(Build build);
    }
}