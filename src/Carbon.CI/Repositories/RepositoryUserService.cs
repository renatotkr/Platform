using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Cloud.Logging;
using Carbon.Data.Expressions;
using Carbon.Security;

namespace Carbon.CI
{
    using static Expression;

    public class RepositoryUserService : IRepositoryUserService
    {
        private readonly CiadDb db;
        private readonly IEventLogger eventLog;

        public RepositoryUserService(CiadDb db, IEventLogger eventLog)
        {
            this.db       = db ?? throw new ArgumentNullException(nameof(db));
            this.eventLog = eventLog ?? throw new ArgumentNullException(nameof(eventLog));
        }

        public Task<IReadOnlyList<RepositoryUser>> ListAsync(IRepository repository)
        {
            #region Preconditions

            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            #endregion 

            return db.RepositoryUsers.QueryAsync(
                expression: And(Eq("repositoryId", repository.Id), IsNull("deleted"))
            );
        }

        public async Task<RepositoryUser> CreateAsync(CreateRepositoryUserRequest request, ISecurityContext context)
        {
            var user = new RepositoryUser(request.RepositoryId, request.UserId, request.Properties);

            await db.RepositoryUsers.InsertAsync(user);

            return user;
        }
    }
}