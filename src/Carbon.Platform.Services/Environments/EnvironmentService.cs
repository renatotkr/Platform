using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Environments
{
    using static Expression;

    public class EnvironmentService : IEnvironmentService
    {
        private readonly PlatformDb db;

        public EnvironmentService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<EnvironmentInfo>> ListAsync(long ownerId)
        {
            return db.Environments.QueryAsync(
                And(Eq("ownerId", ownerId), IsNull("deleted"))
            );
        }

        public async Task<EnvironmentInfo> GetAsync(long id)
        {
            return await db.Environments.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.Environment, id);
        }

        public async Task<EnvironmentInfo> GetAsync(long ownerId, string name)
        {
            #region Preconditions

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            #endregion

            return await db.Environments.QueryFirstOrDefaultAsync(
                Conjunction(Eq("ownerId", ownerId), Eq("name", name), IsNull("deleted"))
            ) ?? throw ResourceError.NotFound(ResourceTypes.Environment, ownerId, name);
        }

        public Task<bool> ExistsAsync(long ownerId, string name)
        {
            return db.Environments.ExistsAsync(
                And(Eq("ownerId", ownerId), Eq("name", name))
            );
        }

        public async Task<EnvironmentInfo> GetAsync(string slug)
        {
            return await db.Environments.QueryFirstOrDefaultAsync(Eq("slug", slug))
                ?? throw new ResourceNotFoundException("environment/" + slug);
        }

        public async Task<EnvironmentInfo> CreateAsync(CreateEnvironmentRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            // Ensure an environment with the provided name doesn't already exist...

            if (await ExistsAsync(request.OwnerId, request.Name))
            {
                throw new ResourceConflictException($"environment/{request.Name} (ownerId: {request.OwnerId})");
            }

            var environment = new EnvironmentInfo(
                id      : await db.Environments.Sequence.NextAsync(),
                name    : request.Name,
                ownerId : request.OwnerId
            );
            
            await db.Environments.InsertAsync(environment);

            return environment;
        }
        
        public async Task<bool> DeleteAsync(IEnvironment environment)
        {
            #region Preconditions

            if (environment == null)
                throw new ArgumentNullException(nameof(environment));

            #endregion

            return await db.Environments.PatchAsync(environment.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}