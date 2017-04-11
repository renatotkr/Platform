using System;
using System.Threading.Tasks;

using Carbon.Platform.Computing;
using Carbon.Versioning;
using Carbon.Platform.Apps;

using Dapper;

namespace Carbon.Platform.CI
{
    public class DeploymentService : IDeploymentService
    {
        private readonly PlatformDb db;

        public DeploymentService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Deployment> StartAsync(IEnvironment env, AppRelease release)
        {
            #region Preconditions

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            #endregion

            var deployment = new Deployment(
              id       : await db.Deployments.GetNextScopedIdAsync(env.Id).ConfigureAwait(false),
              appId    : env.AppId,
              revision : release.Version,
              commitId : release.CommitId,
              status   : DeploymentStatus.Pending
            );

            await db.Deployments.InsertAsync(deployment).ConfigureAwait(false);

            return deployment;
        }

        /*
        private async Task<long> GetNextIdAsync(IEnvironment env)
        {
            using (var connection = db.Context.GetConnection())
            using (var ts = connection.BeginTransaction())
            {
                await connection.ExecuteAsync(
                    @"SELECT `deploymentCount` FROM  `Environments` FOR UPDATE;
                      UPDATE `Environments`
                      SET `deploymentCount` = `deploymentCount` + 1
                      WHERE id = @id", env, ts).ConfigureAwait(false);

                return await connection.ExecuteScalarAsync<int>(
                    "SELECT `deploymentCount` FROM `Environments` WHERE id = @id", env, ts).ConfigureAwait(false);
            }
        }
        */

        public async Task CompleteAsync(Deployment deployment, bool succceded)
        {
            #region Preconditions

            if (deployment == null)
                throw new ArgumentNullException(nameof(deployment));

            #endregion

            deployment.Status = succceded ? DeploymentStatus.Succeeded : DeploymentStatus.Failed;

            deployment.Completed = DateTime.UtcNow;
            
            using (var connection = db.Context.GetConnection())
            {
                await connection.ExecuteAsync(
                    @"UPDATE `Deployments`
                      SET `status` = @status
                          `completed` = NOW()
                      WHERE id = @id", deployment
                 ).ConfigureAwait(false);

                if (succceded)
                {
                    await connection.ExecuteAsync(
                        @"UPDATE `Environments`
                          SET `deploymentId` = @deploymentId
                          WHERE `id` = @id", new {
                            id           = deployment.EnvironmentId,
                            deploymentId = deployment.Id
                        }
                    ).ConfigureAwait(false);
                }
            }
        }

        public async Task CreateTargetsAsync(IDeployment deployment, IHost[] hosts)
        {
            #region Preconditions

            if (deployment == null)
                throw new ArgumentNullException(nameof(deployment));

            if (hosts == null)
                throw new ArgumentNullException(nameof(hosts));

            #endregion

            var targets = new DeploymentTarget[hosts.Length];

            for (var i = 0; i < hosts.Length; i++)
            {
                targets[i] = new DeploymentTarget(
                    deploymentId : deployment.Id, 
                    hostId       : hosts[i].Id,
                    status       : DeploymentStatus.Succeeded
                );
            }

            await db.DeploymentTargets.InsertAsync(targets).ConfigureAwait(false);
        }
    }
}
