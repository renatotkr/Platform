using System;
using System.Threading.Tasks;

using Carbon.Platform;
using Carbon.Platform.Computing;

using Dapper;

namespace Carbon.CI
{
    public class DeploymentService : IDeploymentService
    {
        private readonly CiadDb db;

        public DeploymentService(CiadDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Deployment> StartAsync(
            IEnvironment environment, 
            IRelease release, 
            long creatorId)
        {
            #region Preconditions

            Validate.NotNull(environment, nameof(environment));

            Validate.NotNull(release, nameof(release));
            
            Validate.Id(creatorId, nameof(creatorId));
            
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

            Validate.NotNull(nameof(deployment), nameof(deployment));

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

            Validate.NotNull(deployment, nameof(deployment));

            Validate.NotNull(hosts, nameof(hosts));

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