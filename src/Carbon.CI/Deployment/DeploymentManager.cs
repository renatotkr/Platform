using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Cloud.Logging;
using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Logging;
using Carbon.Platform.Computing;
using Carbon.Platform.Environments;
using Carbon.Security;

namespace Carbon.CI
{
    public class DeploymentManager : IDeploymentManager
    {
        private readonly HostAgentClient hostAgent;
        private readonly ILogger log;
        private readonly IHostService hostService;
        private readonly CiadDb db;
        private readonly IProgramReleaseService releaseService;
        private readonly IEventLogger eventLog;

        public DeploymentManager(
            HostAgentClient api, 
            CiadDb db,
            IHostService hostService,
            IProgramReleaseService releaseService,
            IEventLogger eventLog,
            ILogger log)
        {
            this.hostAgent       = api           ?? throw new ArgumentNullException(nameof(api));
            this.log            = log            ?? throw new ArgumentNullException(nameof(log));
            this.releaseService = releaseService ?? throw new ArgumentNullException(nameof(releaseService));
            this.db             = db             ?? throw new ArgumentNullException(nameof(db));
            this.hostService    = hostService    ?? throw new ArgumentNullException(nameof(hostService));
            this.eventLog       = eventLog       ?? throw new ArgumentNullException(nameof(eventLog));
        }

        public async Task<DeployResult> DeployAsync(DeployRequest request, ISecurityContext context)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            #endregion
            
            var environment = request.Environment;
            var program     = request.Program;

            var release = await releaseService.GetAsync(program.Id, program.Version);
            var hosts   = await hostService.ListAsync(environment);

            var deployment = await StartAsync(
                environment : environment,
                release     : release,
                creatorId   : context.UserId.Value
            );

            var targets = new DeploymentTarget[hosts.Count];
            
            for (var i = 0; i < hosts.Count; i++)
            {
                targets[i] = new DeploymentTarget(
                    deploymentId : deployment.Id,
                    hostId       : hosts[i].Id,
                    status       : DeploymentStatus.Pending
                );
            }

            var results = await ActivateAsync(release, hosts);

            for (var i = 0; i < results.Length; i++)
            {
                var deployResult = results[i];

                targets[i].Status = deployResult.Succeeded 
                    ? DeploymentStatus.Succeeded 
                    : DeploymentStatus.Failed;

                targets[i].Message = deployResult.Message;
            }

            // Complete the deployment & save the targets
            await CompleteAsync(deployment, targets, succceded: true);

            // Log the action
            await eventLog.CreateAsync(new Event(
                action   : "deploy",
                resource : "application#" + release.Id + "@" + release.Version,
                userId   : context.UserId
            ));

            return new DeployResult(true);
        }

        // Activate on a single host
        public async Task<DeployResult> DeployAsync(IProgram program, IHost host)
        {
            #region Preconditions

            if (program == null)
                throw new ArgumentNullException(nameof(program));

            if (host == null)
                throw new ArgumentNullException(nameof(host));

            #endregion

            log.Info($"{host.Id} / activating");

            DeployResult result;

            try
            {
                result = await hostAgent.DeployAsync(program, host);
            }
            catch (Exception ex)
            {
                log.Error($"error activating {host.Id} ({host.Address.ToString()} ... {ex.Message}");

                return new DeployResult(false, ex.Message);
            }

            log.Info(host.Id + " - " + result.Message);

            return result;
        }

        private async Task<DeployResult[]> ActivateAsync(IProgram program, IReadOnlyList<IHost> hosts)
        {
            #region Preconditions

            if (program == null)
                throw new ArgumentNullException(nameof(program));

            if (hosts == null)
                throw new ArgumentNullException(nameof(hosts));

            #endregion

            var tasks = new Task<DeployResult>[hosts.Count];

            for (var i = 0; i < hosts.Count; i++)
            {
                tasks[i] = DeployAsync(program, hosts[i]);
            }

            return await Task.WhenAll(tasks);
        }

        public async Task RestartAsync(IProgram program, IHost host)
        {
            var text = await hostAgent.RestartAsync(program, host);

            log.Info($"{host} : Reloading -- {text}");
        }

        #region Data Acesss

        private async Task<Deployment> StartAsync(
            IEnvironment environment, 
            IProgramRelease release, 
            long creatorId)
        { 
            var deployment = new Deployment(
                id        : await db.Deployments.Sequence.NextAsync(),
                release   : release,
                creatorId : creatorId
            );

            await db.Deployments.InsertAsync(deployment);
 
            return deployment;
        }     
 
        private async Task CompleteAsync(
            Deployment deployment,
            DeploymentTarget[] targets,
            bool succceded)
        {
            #region Preconditions

            if (deployment == null)
                throw new ArgumentNullException(nameof(deployment));

            if (targets == null)
                throw new ArgumentNullException(nameof(targets));
            
            #endregion
 
            deployment.Status = succceded
                ? DeploymentStatus.Succeeded
                : DeploymentStatus.Failed;
 
            deployment.Completed = DateTime.UtcNow;

            // TODO: complete this in a transaction
            // todo: patch all the host versions...

            await db.DeploymentTargets.InsertAsync(targets);

            await db.Deployments.PatchAsync(deployment.Id, changes: new[] {
                Change.Replace("status",    deployment.Status),
                Change.Replace("completed", Expression.Func("NOW"))
            });
         }
 
        #endregion
    }
}
