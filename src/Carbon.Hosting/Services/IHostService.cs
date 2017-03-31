using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Hosting
{
    using Platform.Apps;
    using Storage;

    // e.g. IIS, Apache, SelfHost, etc.

    public interface IHostService
    {
        IEnumerable<IApp> Scan();

        IApp Find(long id); 

        Task DeleteAsync(IApp app);

        Task DeployAsync(IApp app, IPackage package);

        // This will install the program if it doesn't already exist
        Task ActivateAsync(IApp app); 

        Task ReloadAsync(IApp app);

        bool IsDeployed(IApp app);
    }
}