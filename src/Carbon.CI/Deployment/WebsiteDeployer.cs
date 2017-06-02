using System;
using System.Threading.Tasks;
using System.Net;

using Carbon.Logging;
using Carbon.Platform.Computing;
using Carbon.Platform.Web;

namespace Carbon.CI
{
    public class WebsiteDeployer : IWebsiteDeployer
    {
        private readonly ApiClient api;
        private readonly ILogger log;
        private readonly IDeploymentService deploymentService;
        private readonly IHostService hostService;
        private readonly IWebsiteReleaseService releaseService;

        public WebsiteDeployer(
            ApiClient api,
            IWebsiteReleaseService releaseService,
            IDeploymentService deploymentService,
            IHostService hostService,
            ILogger logger)
        {
            this.api               = api               ?? throw new ArgumentNullException(nameof(api));
            this.log               = logger            ?? throw new ArgumentNullException(nameof(logger));
            this.releaseService    = releaseService    ?? throw new ArgumentNullException(nameof(releaseService));
            this.hostService       = hostService       ?? throw new ArgumentNullException(nameof(hostService));
            this.deploymentService = deploymentService ?? throw new ArgumentNullException(nameof(deploymentService));
        }

        public async Task<DeployResult> DeployAsync(DeployWebsiteRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var release = await releaseService.GetAsync(request.WebsiteId, request.WebsiteVersion);

            var deployment = await deploymentService.StartAsync(request.Environment, release, request.InitiatorId);

            var hosts = await hostService.ListAsync(request.Environment).ConfigureAwait(false);
            
            await DeployAsync(release, hosts).ConfigureAwait(false);

            await deploymentService.CompleteAsync(deployment, succceded: true);

            return new DeployResult(true);
        }

        private async Task DeployAsync(WebsiteRelease release, IHost[] hosts)
        {
            #region Preconditions

            Validate.NotNull(release, nameof(release));

            Validate.NotNull(hosts, nameof(hosts));

            #endregion

            var tasks = new Task[hosts.Length];

            for (var i = 0; i < hosts.Length; i++)
            {
                tasks[i] = DeployAsync(release, hosts[i].Address);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task DeployAsync(WebsiteRelease release, IPAddress host)
        {
            #region Preconditions

            Validate.NotNull(release, nameof(release));

            Validate.NotNull(host, nameof(host));

            #endregion

            log.Info($"{host} / activating");

            var result = await api.SendAsync(
                host  : host,
                path : $"/websites/{release.WebsiteId}@{release.Version}/activate"
            ).ConfigureAwait(false);

            log.Info(result);
        }
    }
}