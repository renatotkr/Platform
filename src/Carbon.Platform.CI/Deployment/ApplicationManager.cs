using System;
using System.Threading.Tasks;

using Carbon.Logging;
using Carbon.Platform.Computing;

namespace Carbon.Platform.CI
{
    public class ApplicationManager : IApplicationManager
    {
        private readonly ApiClient api;
        private readonly ILogger log;
        private readonly IEnvironmentService environments;
        private readonly IDeploymentService deployments;
        private readonly IProgramReleaseService programReleases;

        public ApplicationManager(
            ApiClient api, 
            IDeploymentService deployments,
            IEnvironmentService environments,
            IProgramReleaseService programReleases,
            ILogger log)
        {
            this.api             = api ?? throw new ArgumentNullException(nameof(api));
            this.log             = log ?? throw new ArgumentNullException(nameof(log));
            this.programReleases = programReleases ?? throw new ArgumentNullException(nameof(programReleases));
            this.deployments     = deployments ?? throw new ArgumentNullException(nameof(deployments));
            this.environments    = environments ?? throw new ArgumentNullException(nameof(environments));
        }

        public async Task<DeployResult> DeployAsync(DeployApplicationRequest request)
        {
            // TODO: Validation

            var release = await programReleases.GetAsync(request.ApplicationId, request.ApplicationVersion);
            var env     = await environments.GetAsync(request.EnvironmentId);
            var hosts   = await environments.GetHostsAsync(env).ConfigureAwait(false);

            // Create a deployment record
            var deployment = await deployments.StartAsync(env, release, request.CreatorId).ConfigureAwait(false);

            // Activate the release on the host list
            await ActivateAsync(release, hosts).ConfigureAwait(false);

            // Complete the deployment
            await deployments.CompleteAsync(deployment, succceded: true).ConfigureAwait(false);

            // Create the targets
            await deployments.CreateTargetsAsync(deployment, hosts).ConfigureAwait(false);

            return new DeployResult(true);
        }

        // Activate on a single host
        public async Task<DeployResult> DeployAsync(ProgramRelease release, IHost host)
        {
            #region Preconditions

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            #endregion

            log.Info($"{host.Id} / activating");

            string text;

            try
            {
                text = await api.SendAsync(host.Address, $"/apps/{release.ProgramId}@{release.Version}/activate").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.Error($"error activating {host.Id} ({host.Address.ToString()} ... {ex.Message}");

                return new DeployResult(false, ex.Message);
            }

            log.Info(host.Id + " - " + text);

            return new DeployResult(true, text);
        }

        private async Task<DeployResult[]> ActivateAsync(ProgramRelease release, IHost[] hosts)
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

        public async Task RestartAsync(IApplication app, IHost host)
        {
            var text = await api.SendAsync(host.Address, $"/apps/{app.Id}/reload").ConfigureAwait(false);

            log.Info($"{host} : Reloading -- {text}");
        }
    }
}
