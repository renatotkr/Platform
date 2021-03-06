﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Cloud.Logging;
using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Logging;
using Carbon.Platform.Computing;
using Carbon.Platform.Environments;
using Carbon.Platform.Resources;
using Carbon.Security;

namespace Carbon.CI
{
    using static Expression;

    public class DeploymentManager : IDeploymentManager
    {
        private readonly HostAgentClient hostAgent;
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
            this.hostAgent      = api            ?? throw new ArgumentNullException(nameof(api));
            this.releaseService = releaseService ?? throw new ArgumentNullException(nameof(releaseService));
            this.db             = db             ?? throw new ArgumentNullException(nameof(db));
            this.hostService    = hostService    ?? throw new ArgumentNullException(nameof(hostService));
            this.eventLog       = eventLog       ?? throw new ArgumentNullException(nameof(eventLog));
        }

        public Task<IReadOnlyList<Deployment>> ListAsync(IEnvironment environment)
        {
            Ensure.NotNull(environment, nameof(environment));

            return db.Deployments.QueryAsync(
                expression : Eq("environmentId", environment.Id),
                order      : Order.Descending("id")
            );
        }

        public Task<IReadOnlyList<Deployment>> ListAsync(IEnvironment environment, long programId)
        {
            Ensure.NotNull(environment, nameof(environment));

            return db.Deployments.QueryAsync(
                And(
                    Eq("environmentId", environment.Id),
                    Eq("programId", programId)
                ),
                order: Order.Descending("id")
            );
        }

        public async Task<Deployment> GetAsync(long id)
        {
            return await db.Deployments.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.Deployment, id);
        }

        public async Task<Deployment> DeployAsync(DeployRequest request, ISecurityContext context)
        {
            Ensure.NotNull(request, nameof(request));
            Ensure.NotNull(context, nameof(context));
            
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
                resource : "program#" + release.Id + "@" + release.Version,
                userId   : context.UserId
            ));

            return deployment;
        }

        // Activate on a single host
        public async Task<DeployResult> DeployAsync(IProgram program, IHost host)
        {
            Ensure.NotNull(program, nameof(program));
            Ensure.NotNull(host, nameof(host));

            DeployResult result;

            try
            {
                result = await hostAgent.DeployAsync(program, host);
            }
            catch (Exception ex)
            {
                return new DeployResult(false, ex.Message);
            }

            return result;
        }

        private async Task<DeployResult[]> ActivateAsync(IProgram program, IReadOnlyList<IHost> hosts)
        {
            Ensure.NotNull(program, nameof(program));
            Ensure.NotNull(hosts,   nameof(hosts));

            var tasks = new Task<DeployResult>[hosts.Count];

            for (var i = 0; i < hosts.Count; i++)
            {
                tasks[i] = DeployAsync(program, hosts[i]);
            }

            return await Task.WhenAll(tasks);
        }

        public async Task<bool> RestartAsync(IProgram program, IHost host)
        {
            return await hostAgent.RestartAsync(program, host);
        }

        #region Data Acesss

        private async Task<Deployment> StartAsync(
            IEnvironment environment, 
            IProgramRelease release, 
            long creatorId)
        {
            Ensure.NotNull(environment, nameof(environment));

            var deployment = new Deployment(
                id            : await db.Deployments.Sequence.NextAsync(),
                environmentId : environment.Id,
                release       : release,
                creatorId     : creatorId
            );

            await db.Deployments.InsertAsync(deployment);
 
            return deployment;
        }     
 
        private async Task CompleteAsync(
            Deployment deployment,
            DeploymentTarget[] targets,
            bool succceded)
        {
            Ensure.NotNull(deployment, nameof(deployment));
            Ensure.NotNull(targets, nameof(targets));
 
            deployment.Status = succceded
                ? DeploymentStatus.Succeeded
                : DeploymentStatus.Failed;
 
            deployment.Completed = DateTime.UtcNow;

            // TODO: complete this in a transaction
            // todo: patch all the host versions...

            await db.DeploymentTargets.InsertAsync(targets);

            await db.Deployments.PatchAsync(deployment.Id, changes: new[] {
                Change.Replace("status",    deployment.Status),
                Change.Replace("completed", Func("NOW"))
            });
         }
 
        #endregion
    }
}
