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

        Task DeployAsync(IApplication app, SemanticVersion version, JsonObject env, IPackage package);

        // Start
        // Stop

        Task RestartAsync(IApplication app);

        Task DeleteAsync(IApplication app);

        bool IsDeployed(IApplication app, SemanticVersion version);
    }
}