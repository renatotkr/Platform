using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Hosting
{
    using Carbon.Json;
    using Carbon.Versioning;
    using Platform.Apps;
    using Storage;

    public interface IHostService
    {
        IEnumerable<IApp> Scan();

        IApp Find(long id);

        Task DeployAsync(IApp app, SemanticVersion version, JsonObject env, IPackage package);

        // Start
        // Stop

        Task RestartAsync(IApp app);

        Task DeleteAsync(IApp app);

        bool IsDeployed(IApp app, SemanticVersion version);
    }
}

// e.g. IIS, Upstart