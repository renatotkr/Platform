using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            return await db.Environments.FindAsync(id).ConfigureAwait(false)
                ?? throw ResourceError.NotFound(ResourceTypes.Environment, id);
        }

        public async Task<EnvironmentInfo> GetAsync(long ownerId, string name)
        {
            return await db.Environments.QueryFirstOrDefaultAsync(
                And(Eq("ownerId", ownerId), Eq("name", name))
            ).ConfigureAwait(false) ?? throw ResourceError.NotFound(ResourceTypes.Environment, ownerId, name);
        }

        public async Task<bool> ExistsAsync(long ownerId, string name)
        {
            return await db.Environments.CountAsync(
                And(Eq("ownerId", ownerId), Eq("name", name))
            ).ConfigureAwait(false) > 0;
        }

        public async Task<EnvironmentInfo> GetAsync(string slug)
        {
            return await db.Environments.QueryFirstOrDefaultAsync(Eq("slug", slug)).ConfigureAwait(false)
                ?? throw new ResourceNotFoundException("environment/" + slug);
        }

        public async Task<EnvironmentInfo> CreateAsync(CreateEnvironmentRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            // Ensure an environment with the provided name doesn't already exist...

            if (await ExistsAsync(request.OwnerId, request.Name).ConfigureAwait(false))
            {
                throw new Exception($"environment named '{request.Name}' already exists by '{request.OwnerId}'");
            }

            var environment = new EnvironmentInfo(
                id      : db.Environments.Sequence.Next(),
                name    : request.Name,
                ownerId : request.OwnerId
            );
            
            await db.Environments.InsertAsync(environment);

            return environment;
        }
    }
}