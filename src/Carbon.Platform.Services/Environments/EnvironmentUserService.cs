using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;

namespace Carbon.Platform.Environments
{
    public class EnvironmentUserService : IEnvironmentUserService
    {
        private readonly PlatformDb db;

        public EnvironmentUserService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<EnvironmentUser>> ListAsync(IEnvironment environment)
        {
            Validate.NotNull(environment, nameof(environment));

            return db.EnvironmentUsers.QueryAsync(
                Expression.And(Expression.Eq("environmentId", environment.Id), Expression.IsNull("deleted"))
            );
        }

        public Task<EnvironmentUser> GetAsync(long environmentId, long userId)
        {
            return db.EnvironmentUsers.FindAsync((environmentId, userId));
        }

        public async Task<EnvironmentUser> CreateAsync(CreateEnvironmentUserRequest request)
        {
            Validate.NotNull(request, nameof(request));

            var environmentUser = new EnvironmentUser(
                environmentId : request.EnvironmentId,
                userId        : request.UserId,
                roles         : request.Roles
            );
            
            await db.EnvironmentUsers.InsertAsync(environmentUser);

            return environmentUser;
        }

        public async Task DeleteAsync(EnvironmentUser environmentUser)
        {
            Validate.NotNull(environmentUser, nameof(environmentUser));

            await db.EnvironmentUsers.PatchAsync((environmentUser.EnvironmentId, environmentUser.UserId), new[] {
                Change.Replace("deleted", Expression.Func("NOW"))
            });
        }
    }
}