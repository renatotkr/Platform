using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Hosting
{
    using Platform.Apps;
    using Packaging;

    // This could be IIS, Apache, Unicorn, or a Self Host

    public interface IHostService
    {
        IEnumerable<IApp> Scan();

        IApp Find(long id); 

        Task DeleteAsync(IApp app);

        Task DeployAsync(IApp app, Package package);

        // This will install the program if it doesn't already exist
        Task ActivateAsync(IApp app); 

        Task ReloadAsync(IApp app);

        bool IsDeployed(IApp app);
    }
}