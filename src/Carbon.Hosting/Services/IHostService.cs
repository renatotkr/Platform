using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Json;
using Carbon.Platform.Computing;
using Carbon.Versioning;

namespace Carbon.Hosting
{
    using Storage;
    
    public interface IHostService
    {
        IEnumerable<IApplication> Scan();

        IApplication Find(long id);

        Task DeployAsync(IApplication application, SemanticVersion version, JsonObject env, IPackage package);

        // Start
        // Stop

        Task RestartAsync(IApplication application);

        Task DeleteAsync(IApplication application);

        bool IsDeployed(IApplication application, SemanticVersion version);
    }
}

// e.g. IIS, Upstart