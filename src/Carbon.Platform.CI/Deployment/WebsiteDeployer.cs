using System;
using System.Threading.Tasks;
using System.Net;

using Carbon.Logging;
using Carbon.Platform.Computing;
using Carbon.Platform.Web;
using Carbon.Protection;
using Carbon.Versioning;

using Dapper;

namespace Carbon.Platform.CI
{
    public class WebsiteDeployer : ApiBase, IWebsiteDeployer
    {
        private readonly PlatformDb db;
        private readonly ILogger log;
        private readonly EnvironmentService envService;

        public WebsiteDeployer(SecretKey key, int port, PlatformDb db, ILogger logger)
            : base(key, port)
        {
            this.db         = db     ?? throw new ArgumentNullException(nameof(db));
            this.log        = logger ?? throw new ArgumentNullException(nameof(logger));
            this.envService = new EnvironmentService(db);
        }

        public async Task<DeployResult> DeployAsync(IWebsite website, SemanticVersion version, IEnvironment env)
        {
            #region Preconditions

            if (website == null) throw new ArgumentNullException(nameof(website));

            #endregion

            var hosts = await envService.GetHostsAsync(env).ConfigureAwait(false);
            
            await DeployAsync(website, version, hosts).ConfigureAwait(false);

            using (var connection = db.Context.GetConnection())
            {
                connection.Execute(
                    @"UPDATE Websites
                      SET revision = @revision
                      WHERE id = @id", new {
                        id = env.Id,
                        revision = version.ToString()
                    });
            }
            
            return new DeployResult(true);
        }

        private async Task DeployAsync(IWebsite website, SemanticVersion version, IHost[] hosts)
        {
            var tasks = new Task[hosts.Length];

            for (var i = 0; i < hosts.Length; i++)
            {
                tasks[i] = DeployAsync(website, version, hosts[i].Address);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task DeployAsync(IWebsite website, SemanticVersion version, IPAddress host)
        {
            #region Preconditions

            if (website == null)
                throw new ArgumentNullException(nameof(website));

            if (host == null)
                throw new ArgumentNullException(nameof(host));

            #endregion

            log.Info($"{host} / activating");

            var result = await SendAsync(host, $"/websites/{website.Id}@{version}/activate").ConfigureAwait(false);

            log.Info(result);
        }
    }
}