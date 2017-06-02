using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;

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
        
        public async Task<WebsiteInfo> CreateAsync(CreateWebsiteRequest request)
        {
            var website = new WebsiteInfo(
                id            : db.Websites.Sequence.Next(),
                name          : request.Name,
                repositoryId  : request.RepositoryId,
                environmentId : request.EnvironmentId,
                ownerId       : request.OwnerId
            );

            await db.Websites.InsertAsync(website).ConfigureAwait(false);

            return website;
        }
    }
}