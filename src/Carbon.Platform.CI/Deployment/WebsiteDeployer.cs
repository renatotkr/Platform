using System;
using System.Threading.Tasks;
using System.Net;

using Carbon.Logging;
using Carbon.Platform.Computing;
using Carbon.Platform.Services;
using Carbon.Platform.Web;

namespace Carbon.Platform.CI
{
    public class WebsiteDeployer : IWebsiteDeployer
    {
        private readonly ApiClient api;
        private readonly ILogger log;
        private readonly EnvironmentService environments;
        private readonly DeploymentService deployments;
        private readonly IWebsiteService websiteService;

        public WebsiteDeployer(ApiClient api, IWebsiteService websiteService, PlatformDb db, ILogger logger)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.log = logger ?? throw new ArgumentNullException(nameof(logger));
            this.websiteService = websiteService;

            this.environments = new EnvironmentService(db);
            this.deployments  = new DeploymentService(db);
        }

        public async Task<DeployResult> DeployAsync(DeployWebsiteRequest request)
        {
            var env = await environments.GetAsync(request.EnvironmentId);
            var release = await websiteService.GetReleaseAsync(request.WebsiteId, request.WebsiteVersion);

            var deployment = await deployments.StartAsync(env, release, request.InitiatorId);

            var hosts = await environments.GetHostsAsync(env).ConfigureAwait(false);
            
            await DeployAsync(release, hosts).ConfigureAwait(false);

            await deployments.CompleteAsync(deployment, true);

            return new DeployResult(true);
        }

        private async Task DeployAsync(WebsiteRelease release, IHost[] hosts)
        {
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

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            if (host == null)
                throw new ArgumentNullException(nameof(host));

            #endregion

            log.Info($"{host} / activating");

            var result = await api.SendAsync(host, $"/websites/{release.WebsiteId}@{release.Version}/activate").ConfigureAwait(false);

            log.Info(result);
        }
    }
}