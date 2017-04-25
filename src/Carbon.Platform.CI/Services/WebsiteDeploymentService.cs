using System;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.CI;

using Dapper;

namespace Carbon.Platform.Web
{
    public class WebsiteDeploymentService
    {
        private readonly WebDb db;

        public WebsiteDeploymentService(WebDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<WebsiteDeployment> StartAsync(
            WebsiteRelease release, 
            IEnvironment env,
            long creatorId)
        {
            #region Preconditions

            if (release == null)
                throw new ArgumentNullException(nameof(release));

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            #endregion

            var deployment = new WebsiteDeployment(
                id        : await DeploymentId.GetNextAsync(db.Context, env).ConfigureAwait(false),
                websiteId : release.WebsiteId,
                revision  : release.Version,
                commitId  : release.CommitId,
                creatorId : creatorId,
                status    : DeploymentStatus.Pending
            );

            await db.WebsiteDeployments.InsertAsync(deployment);

            return deployment;
        }

        public async Task CompleteAsync(WebsiteDeployment deployment, bool successsful)
        {
            #region Preconditions

            if (deployment == null)
                throw new ArgumentNullException(nameof(deployment));

            #endregion

            using (var connection = db.Context.GetConnection())
            using (var ts = connection.BeginTransaction())
            {
                await connection.ExecuteAsync(
                    @"UPDATE WebsiteDeployments
                      SET status = @status,
                          completed = NOW()
                      WHERE id = @id", new
                    {
                        id = deployment.Id,
                        status = successsful ? DeploymentStatus.Succeeded : DeploymentStatus.Failed
                    }, ts);

                await connection.ExecuteAsync(
                    @"UPDATE Websites
                      SET deploymentId = @deploymentId
                      WHERE id = @id", new {
                        id = deployment.WebsiteId,
                        deploymentId = deployment.Id
                    }, ts);
            }
        }
    }
}
