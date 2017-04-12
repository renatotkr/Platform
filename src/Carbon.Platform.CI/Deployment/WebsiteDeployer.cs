using System;
using System.Threading.Tasks;
using System.Net;

using Carbon.Logging;
using Carbon.Platform.Computing;
using Carbon.Platform.Services;
using Carbon.Platform.Web;
using Carbon.Protection;

namespace Carbon.Platform.CI
{
    public class WebsiteDeployer : ApiBase, IWebsiteDeployer
    {
        private readonly PlatformDb db;
        private readonly ILogger log;
        private readonly EnvironmentService envService;
        private readonly WebsiteService websiteService;

        public WebsiteDeployer(SecretKey key, int port, PlatformDb db, ILogger logger)
            : base(key, port)
        {
            this.db         = db     ?? throw new ArgumentNullException(nameof(db));
            this.log        = logger ?? throw new ArgumentNullException(nameof(logger));
            this.envService = new EnvironmentService(db);

            this.websiteService = new WebsiteService(new WebDb(db.Context));
        }

        public async Task<DeployResult> DeployAsync(WebsiteRelease release, IEnvironment env)
        {
            #region Preconditions

            if (release == null) throw new ArgumentNullException(nameof(release));

            #endregion
            
            var deployment = await websiteService.StartDeployment(release, env, 0);

            var hosts = await envService.GetHostsAsync(env).ConfigureAwait(false);
            
            await DeployAsync(release, hosts).ConfigureAwait(false);

            await websiteService.CompleteDeploymentAsync(deployment, true);

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

            var result = await SendAsync(host, $"/websites/{release.WebsiteId}@{release.Version}/activate").ConfigureAwait(false);

            log.Info(result);
        }
    }
}