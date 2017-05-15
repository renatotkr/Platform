using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.VersionControl;
using Carbon.Versioning;

using Dapper;

namespace Carbon.Platform.Web
{
    using static Expression;

    public class WebsiteService : IWebsiteService
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

        public Task<WebsiteInfo> FindAsync(string name)
        {
            return db.Websites.QueryFirstOrDefaultAsync(Eq("name", name));
        }

        public Task<IReadOnlyList<WebsiteInfo>> ListAsync(long ownerId)
        {
            return db.Websites.QueryAsync(
                And(Eq("ownerId", ownerId), IsNull("deleted")),
                Order.Ascending("name")
            );
        }

        public Task<WebsiteRelease> GetReleaseAsync(long websiteId, SemanticVersion version)
        {
            return db.WebsiteReleases.QueryFirstOrDefaultAsync(
                And(Eq("websiteId", websiteId), Eq("version", version))
             );
        }

        public Task<IReadOnlyList<WebsiteRelease>> GetReleasesAsync(long websiteId)
        {
            return db.WebsiteReleases.QueryAsync(
                 Eq("websiteId", websiteId),
                 Order.Descending("version")
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

            // TODO: Ensure that the version does not already exist

            var releaseId  = await GetNextReleaseIdAsync(website);

            var release = new WebsiteRelease(releaseId, website, version, sha256, commit, creatorId);
            
            await db.WebsiteReleases.InsertAsync(release).ConfigureAwait(false);

            return release;
        }

        private async Task<long> GetNextReleaseIdAsync(IWebsite website)
        {
            using (var connection = db.Context.GetConnection())
            using (var ts = connection.BeginTransaction())
            {
                var result = await connection.ExecuteScalarAsync<int>(
                    @"SELECT `releaseCount` FROM `Websites` WHERE id = @id FOR UPDATE;
                      UPDATE `Websites`
                      SET `releaseCount` = `releaseCount` + 1
                      WHERE id = @id", website, ts).ConfigureAwait(false);

                ts.Commit();

                return result + 1;
            }
        }
        
        public async Task<WebsiteInfo> CreateAsync(
            string name, 
            long ownerId,
            IEnvironment env,
            IRepository repository)
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
                id            : db.Websites.Sequence.Next(),
                name          : name,
                repositoryId  : repository.Id,
                environmentId : env.Id,
                ownerId       : ownerId
            );

            await db.Websites.InsertAsync(website).ConfigureAwait(false);

            return website;
        }
    }
}