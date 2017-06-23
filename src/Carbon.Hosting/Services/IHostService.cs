using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Platform.Computing;

namespace Carbon.Hosting
{
    using Storage;
    
    public interface IHostService
    {
        IEnumerable<IProgram> Scan();

        IProgram Find(long id);

        Task DeployAsync(IProgram app, IPackage package);

        // Start
        // Stop

        Task RestartAsync(IProgram app);

        Task DeleteAsync(IProgram app);

        bool IsDeployed(IProgram app);
    }
}