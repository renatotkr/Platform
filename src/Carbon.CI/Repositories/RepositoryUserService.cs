using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Cloud.Logging;
using Carbon.Data;
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
            Ensure.NotNull(repository, nameof(repository));

            return db.RepositoryUsers.QueryAsync(
                expression: And(Eq("repositoryId", repository.Id), IsNull("deleted"))
            );
        }

        public Task<IReadOnlyList<RepositoryUser>> ListHavingUserIdAsync(long userId)
        {  
            return db.RepositoryUsers.QueryAsync(
                expression: And(Eq("userId", userId), IsNull("deleted"))
            );
        }

        public async Task<RepositoryUser> CreateAsync(CreateRepositoryUserRequest request, ISecurityContext context)
        {
            Ensure.NotNull(request, nameof(request));
            Ensure.NotNull(context, nameof(context));

            var record = new RepositoryUser(
                repositoryId : request.RepositoryId,
                userId       : request.UserId,
                privileges   : request.Privileges, 
                path         : request.Path
            );

            await db.RepositoryUsers.InsertAsync(record);

            return record;
        }
        
        public async Task<bool> DeleteAsync(RepositoryUser record, ISecurityContext context)
        {
            Ensure.NotNull(record, nameof(record));
            Ensure.NotNull(context, nameof(context));

            var result = await db.RepositoryUsers.PatchAsync((record.RepositoryId, record.UserId), new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;

            if (result)
            {
                await eventLog.CreateAsync(new Event("delete", $"borg:repository/{record.RepositoryId}/user/{record.UserId}"));
            }

            return result;
        }
    }
}