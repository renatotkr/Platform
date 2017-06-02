using System;
using System.Threading.Tasks;

using Carbon.Platform.Computing;

using Dapper;

namespace Carbon.CI
{
    public class DeploymentService : IDeploymentService
    {
        private readonly PlatformDb db;

        public DeploymentService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Deployment> StartAsync(
            IEnvironment environment, 
            IRelease release, 
            long creatorId)
        {
            #region Preconditions

            if (environment == null)
                throw new ArgumentNullException(nameof(environment));

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            if (creatorId <= 0)
                throw new ArgumentOutOfRangeException(nameof(creatorId), creatorId, "Must be > 0");

            #endregion

            var deployment = new Deployment(
              id          : await DeploymentId.NextAsync(db.Context, environment).ConfigureAwait(false),
              release     : release,
              initiatorId : creatorId
            );

            await db.Deployments.InsertAsync(deployment).ConfigureAwait(false);

            return deployment;
        }     

        public async Task CompleteAsync(Deployment deployment, bool succceded)
        {
            #region Preconditions

            if (deployment == null)
                throw new ArgumentNullException(nameof(deployment));

            #endregion

            deployment.Status = succceded
                ? DeploymentStatus.Succeeded
                : DeploymentStatus.Failed;

            deployment.Completed = DateTime.UtcNow;
            
            using (var connection = db.Context.GetConnection())
            {
                await connection.ExecuteAsync(
                    @"UPDATE `Deployments`
                      SET `status` = @status
                          `completed` = NOW()
                      WHERE id = @id", deployment
                 ).ConfigureAwait(false);
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