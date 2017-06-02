using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Platform.Computing;

namespace Carbon.Hosting
{
    using Storage;
    
    public interface IHostService
    {
        IEnumerable<IApplication> Scan();

        IApplication Find(long id);

        Task DeployAsync(IApplication app, IPackage package);

        // Start
        // Stop

        Task RestartAsync(IApplication app);

        Task DeleteAsync(IApplication app);

        bool IsDeployed(IApplication app);
    }
}