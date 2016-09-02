using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Hosting
{
    using Packaging;
    using Platform;
    using Programming;

    // This could be IIS, Apache, Unicorn, or a Self Host

    public interface IProgramHost
    {
        IEnumerable<Process> Scan();

        Process Find(string slug); // TODO (id)

        // Task InstallAsync(IProgram program);

        Task DeleteAsync(IProgram program);

        Task DeployAsync(IProgram program, Package package);

        Task<Process> ActivateAsync(IProgram app);

        Task ReloadAsync(IProgram app);

        bool IsDeployed(IProgram app);
    }
}