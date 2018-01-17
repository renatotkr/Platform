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

        public Task<int> CountAsync(long ownerId)
        {
            return db.Environments.CountAsync(
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
            Ensure.NotNull(name, nameof(name));

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
            Ensure.Object(request, nameof(request));

            if (await ExistsAsync(request.OwnerId, request.Name))
            {
                throw new ResourceConflictException($"environment/{request.Name}");
            }
            
            var environment = new EnvironmentInfo(
                id         : await db.Environments.Sequence.NextAsync(),
                name       : request.Name,
                ownerId    : request.OwnerId,
                properties : request.Properties
            );
            
            await db.Environments.InsertAsync(environment);

            return environment;
        }
        
        public async Task<bool> DeleteAsync(IEnvironment environment)
        {
            Ensure.NotNull(environment, nameof(environment));
            
            return await db.Environments.PatchAsync(environment.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}