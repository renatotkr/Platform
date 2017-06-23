using System;
using System.Threading.Tasks;
using System.Linq;

using Carbon.Logging;
using Carbon.Platform.Computing;

namespace Carbon.CI
{
    public class ProgramManager : IProgramManager
    {
        private readonly ApiClient api;
        private readonly ILogger log;
        private readonly IHostService hostService;
        private readonly IDeploymentService deployments;
        private readonly IProgramReleaseService programReleases;

        public ProgramManager(
            ApiClient api, 
            IDeploymentService deploymentService,
            IHostService hostService,
            IProgramReleaseService programReleases,
            ILogger log)
        {
            this.api             = api               ?? throw new ArgumentNullException(nameof(api));
            this.log             = log               ?? throw new ArgumentNullException(nameof(log));
            this.programReleases = programReleases   ?? throw new ArgumentNullException(nameof(programReleases));
            this.deployments     = deploymentService ?? throw new ArgumentNullException(nameof(deploymentService));
            this.hostService     = hostService       ?? throw new ArgumentNullException(nameof(hostService));
        }

        public async Task<DeployResult> DeployAsync(DeployRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion
            
            var environment = request.Environment;

            var release = await programReleases.GetAsync(request.ProgramId, request.ApplicationVersion);
            var hosts   = (await hostService.ListAsync(environment).ConfigureAwait(false)).ToArray();

            // Create a deployment record
            var deployment = await deployments.StartAsync(environment, release, request.InitiatorId).ConfigureAwait(false);

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
                text = await api.SendAsync(host.Address, $"/programs/{release.ProgramId}@{release.Version}/activate").ConfigureAwait(false);
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

        public async Task RestartAsync(IProgram app, IHost host)
        {
            var text = await api.SendAsync(host.Address, $"/programs/{app.Id}/restart").ConfigureAwait(false);

            log.Info($"{host} : Reloading -- {text}");
        }
    }
}
