using System;
using System.Threading.Tasks;

using Carbon.Logging;
using Carbon.Platform.Computing;
using Carbon.Versioning;

namespace Carbon.Platform.CI
{
    using Platform;
    using Platform.Apps;
    using Platform.Logs;
    using Protection;

    public class AppDeployer : ApiBase, IAppDeployer
    {
        private readonly PlatformDb db;
        private readonly ILogger log;
        private readonly EnvironmentService backendService;
        private readonly IDeploymentService ci;

        public AppDeployer(SecretKey key, int port, PlatformDb db, ILogger log)
            : base(key, port)
        {
            this.db             = db   ?? throw new ArgumentNullException(nameof(db));
            this.log            = log ?? throw new ArgumentNullException(nameof(log));
            this.ci             = new DeploymentService(db);
            this.backendService = new EnvironmentService(db);
        }

        public async Task<DeployResult> DeployAsync(IApp app, SemanticVersion version, IEnvironment env)
        {
            #region Preconditions

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            #endregion

            // Create a deployment record
            var deployment = await ci.StartAsync(env, app, version).ConfigureAwait(false);

            var hosts = await backendService.GetHostsAsync(env).ConfigureAwait(false);

            await ActivateAsync(app, version, hosts).ConfigureAwait(false);

            // Create the targets
            await ci.CreateTargetsAsync(deployment, hosts).ConfigureAwait(false);

            // Complete the deployment
            await ci.CompleteAsync(deployment, succceded: true).ConfigureAwait(false);

            return new DeployResult(true);
        }

        // Activate on a single host
        public async Task<DeployResult> DeployAsync(IApp app, SemanticVersion version, IHost host)
        {
            #region Preconditions

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            #endregion

            log.Info($"{host.Id} / activating");

            string text;

            try
            {
                text = await SendAsync(host.Address, $"/apps/{app.Id}@{version}/activate").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.Error($"error activating {host.Id} ({host.Address.ToString()} ... {ex.Message}");

                return new DeployResult(false, ex.Message);
            }

            log.Info(host.Id + " - " + text);

            // TODO: Change to a deploytarget
         
            var e = new Activity(app, ActivityType.Deploy) {
                Details = new Json.JsonObject {
                    { "hostId"   , host.Id },
                    { "revision" , version.ToString() }
                }
            };

            // Message = text

            await db.Activities.InsertAsync(e).ConfigureAwait(false);

            return new DeployResult(true, text);
        }

        private async Task<DeployResult[]> ActivateAsync(IApp app, SemanticVersion version, IHost[] hosts)
        {
            #region Preconditions

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (hosts == null)
                throw new ArgumentNullException(nameof(hosts));

            #endregion

            var tasks = new Task<DeployResult>[hosts.Length];

            for (var i = 0; i < hosts.Length; i++)
            {
                tasks[i] = DeployAsync(app, version, hosts[i]);
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
