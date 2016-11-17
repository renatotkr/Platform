using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Hosting
{
    using Computing;
    using Packaging;

    // This could be IIS, Apache, Unicorn, or a Self Host

    public interface IAppHost
    {
        IEnumerable<Process> Scan();

        Process Find(string slug); // TODO (id)

        Task DeleteAsync(IProgram program);

        Task DeployAsync(IProgram program, Package package);

        // This will install the program if it doesn't already exist
        Task<Process> ActivateAsync(IProgram app); 

        Task ReloadAsync(IProgram app);

        bool IsDeployed(IProgram app);
    }
}