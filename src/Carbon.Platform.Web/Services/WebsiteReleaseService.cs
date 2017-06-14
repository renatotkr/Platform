using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Versioning;

using Dapper;

namespace Carbon.Platform.Web
{
    using static Expression;

    public class WebsiteReleaseService : IWebsiteReleaseService
    {
        private readonly WebDb db;

        public WebsiteReleaseService(WebDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<WebsiteRelease> GetAsync(long websiteId, SemanticVersion version)
        {
            return db.WebsiteReleases.QueryFirstOrDefaultAsync(
                And(Eq("websiteId", websiteId), Eq("version", version))
             );
        }

        public async Task<bool> ExistsAsync(long websiteId, SemanticVersion version)
        {
            return await db.WebsiteReleases.CountAsync(
                And(Eq("websiteId", websiteId), Eq("version", version))
             ).ConfigureAwait(false) > 0;
        }

        public Task<IReadOnlyList<WebsiteRelease>> ListAsync(long websiteId)
        {
            return db.WebsiteReleases.QueryAsync(
                 Eq("websiteId", websiteId),
                 Order.Descending("version")
             );
        }

        public async Task<WebsiteRelease> CreateAsync(CreateWebsiteReleaseRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (await ExistsAsync(request.Website.Id, request.Version).ConfigureAwait(false))
                throw new Exception($"website#{request.Website.Id}@{request.Version} already exists");

            #endregion

            var release = new WebsiteRelease(
                id        : await GetNextId(request.Website),
                website   : request.Website,
                version   : request.Version, 
                package   : request.Package, 
                commit    : request.Commit, 
                creatorId : request.CreatorId
            );
            
            await db.WebsiteReleases.InsertAsync(release).ConfigureAwait(false);

            return release;
        }

        #region Helpers

        private async Task<long> GetNextId(IWebsite website)
        {
            using (var connection = db.Context.GetConnection())
            {
                var result = await connection.ExecuteScalarAsync<int>(
                    @"SELECT `releaseCount` FROM `Websites` WHERE id = @id FOR UPDATE;
                      UPDATE `Websites`
                      SET `releaseCount` = `releaseCount` + 1
                      WHERE id = @id", website).ConfigureAwait(false);

                return result + 1;
            }
        }

        #endregion
    }
}