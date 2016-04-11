using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Hosting
{
    // This could be IIS, Apache, Unicorn, or a Self Host

    public interface IAppHost
    {
        IEnumerable<AppInstance> Scan();

        AppInstance Find(string name); // TODO (id)

        Task CreateAsync(IApp app);

        Task DeleteAsync(IApp app);

        Task DeployAsync(IApp app, int version, Package package);

        Task<AppInstance> ActivateAsync(IApp app, int version);

        Task ReloadAsync(IApp app);

        bool IsDeployed(IApp app, int version);
    }
}