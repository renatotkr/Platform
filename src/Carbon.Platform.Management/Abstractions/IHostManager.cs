using System;
using System.Threading.Tasks;

using Carbon.Platform.Computing;
using Carbon.Platform.Environments;
using Carbon.Security;

namespace Carbon.Platform.Management
{
    public interface IHostManager
    {
        Task<IHost[]> LaunchAsync(LaunchHostRequest launchRequest, ISecurityContext context);

        Task<HostInfo> RegisterAsync(RegisterHostRequest request);

        Task RunCommandAsync(string commandText, IEnvironment environment, ISecurityContext context);

        Task StartAsync(HostInfo host, TimeSpan cooldown);

        Task StopAsync(HostInfo host, TimeSpan cooldown);

        Task RebootAsync(HostInfo host, TimeSpan cooldown);

        Task TerminateAsync(HostInfo host, TimeSpan cooldown, ISecurityContext context);

        Task TransitionStateAsync(HostInfo host, HostStatus newStatus);
    }
}