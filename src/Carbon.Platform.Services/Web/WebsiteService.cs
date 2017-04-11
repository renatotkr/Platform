using Carbon.Platform.VersionControl;
using Carbon.Versioning;
using System;

using Carbon.Data.Expressions;
using Dapper;

using System.Threading.Tasks;

namespace Carbon.Platform.Web
{
    using static Expression;

    public class WebsiteService
    {
        private readonly WebDb db;

        public WebsiteService(WebDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<WebsiteInfo> GetAsync(long id)
        {
            return db.Websites.FindAsync(id);
        }

        public Task<WebsiteInfo> FindAsync(long ownerId, string name)
        {
            return db.Websites.QueryFirstOrDefaultAsync(
                And(Eq("ownerId", ownerId), Eq("name", name))
             );
        }

        public Task<WebsiteRelease> GetReleaseAsync(long websiteId, SemanticVersion version)
        {
            return db.WebsiteReleases.QueryFirstOrDefaultAsync(
                And(Eq("websiteId", websiteId), Eq("version", version))
             );
        }

        public async Task<WebsiteRelease> CreateReleaseAsync(
            IWebsite website, 
            SemanticVersion version,
            IRepositoryCommit commit,
            byte[] sha256,
            long creatorId)
        {
            #region Preconditions

            if (website == null)
                throw new ArgumentNullException(nameof(website));

            if (commit == null)
                throw new ArgumentNullException(nameof(commit));

            if (sha256 == null)
                throw new ArgumentNullException(nameof(sha256));

            #endregion

            var release = new WebsiteRelease(website, version, sha256, commit, creatorId);
            
            await db.WebsiteReleases.InsertAsync(release).ConfigureAwait(false);

            return release;
        }

        public async Task<WebsiteInfo> CreateAsync(string name, IRepository repository, IEnvironment env, long ownerId)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            #endregion

            var website = new WebsiteInfo(
                id: db.Context.GetNextId<WebsiteInfo>(),
                name: name, 
                repositoryId: repository.Id,
                ownerId: ownerId)
            {
                EnvironmentId = env.Id
            };


            await db.Websites.InsertAsync(website).ConfigureAwait(false);

            return website;
        }
        
        public async Task<WebsiteDeployment> StartDeployment(
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

            // TODO: Get the next environmentDeploymentId

            var deployment = new WebsiteDeployment(
                id        : await db.WebsiteDeployments.GetNextScopedIdAsync(release.WebsiteId),
                websiteId : release.WebsiteId,
                revision  : release.Version,
                commitId  : release.CommitId,
                creatorId : creatorId
            );

            await db.WebsiteDeployments.InsertAsync(deployment);

            return deployment;
        }

        public async Task CompleteDeploymentAsync(WebsiteDeployment deployment, bool successsful)
        {
            using (var connection = db.Context.GetConnection())
            {
                await connection.ExecuteAsync(
                    @"UPDATE WebsiteDeployments
                      SET status = @status,
                          completed = NOW()
                      WHERE id = @id", deployment);

                await connection.ExecuteAsync(
                    @"UPDATE Websites
                      SET deploymentId = @deploymentId
                      WHERE id = @id", new {
                        id = deployment.WebsiteId,
                        deploymentId = deployment.Id
                    });
            }


        }
    }
}
