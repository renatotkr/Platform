using System;
using System.Threading.Tasks;

using Carbon.Logging;
using Carbon.Platform.Computing;
using Carbon.Platform.Services;

namespace Carbon.Platform.CI
{
    using Platform;
    using Platform.Apps;
    using Protection;

    public class AppDeployer : ApiBase, IAppDeployer
    {
        private readonly PlatformDb db;
        private readonly ILogger log;
        private readonly EnvironmentService envService;
        private readonly IDeploymentService ci;

        public AppDeployer(SecretKey key, int port, PlatformDb db, ILogger log)
            : base(key, port)
        {
            this.db         = db   ?? throw new ArgumentNullException(nameof(db));
            this.log        = log ?? throw new ArgumentNullException(nameof(log));
            this.ci         = new AppDeploymentService(db);
            this.envService = new EnvironmentService(db);
        }

        public async Task<DeployResult> DeployAsync(IAppRelease release, IEnvironment env)
        {
            #region Preconditions

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            #endregion

            // Create a deployment record
            var deployment = await ci.StartAsync(env, release).ConfigureAwait(false);

            var hosts = await envService.GetHostsAsync(env).ConfigureAwait(false);

            await ActivateAsync(release, hosts).ConfigureAwait(false);

            // Complete the deployment
            await ci.CompleteAsync(deployment, succceded: true).ConfigureAwait(false);

            // Create the targets
            await ci.CreateTargetsAsync(deployment, hosts).ConfigureAwait(false);

            return new DeployResult(true);
        }

        // Activate on a single host
        public async Task<DeployResult> DeployAsync(IAppRelease release, IHost host)
        {
            #region Preconditions

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            #endregion

            log.Info($"{host.Id} / activating");

            string text;

            try
            {
                text = await SendAsync(host.Address, $"/apps/{release.AppId}@{release.Version}/activate").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.Error($"error activating {host.Id} ({host.Address.ToString()} ... {ex.Message}");

                return new DeployResult(false, ex.Message);
            }

            log.Info(host.Id + " - " + text);

            return new DeployResult(true, text);
        }

        private async Task<DeployResult[]> ActivateAsync(IAppRelease release, IHost[] hosts)
        {
            #region Preconditions

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            if (hosts == null)
                throw new ArgumentNullException(nameof(hosts));

            #endregion

            var tasks = new Task<DeployResult>[hosts.Length];

            for (var i = 0; i < hosts.Length; i++)
            {
                tasks[i] = DeployAsync(release, hosts[i]);
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            return results;
        }

        public async Task RestartAsync(IApp app, IHost host)
        {
            var text = await SendAsync(host.Address, $"/apps/{app.Id}/reload").ConfigureAwait(false);

            log.Info($"{host} : Reloading -- {text}");
        }
    }
}
