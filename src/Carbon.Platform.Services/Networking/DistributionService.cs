using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Environments;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    using static Expression;

    public class DistributionService : IDistributionService
    {
        private readonly PlatformDb db;

        public DistributionService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<Distribution>> ListAsync(long ownerId)
        {
            return db.Distributions.QueryAsync(
                And(Eq("ownerId", ownerId), IsNull("deleted"))
            );
        }

        public Task<IReadOnlyList<Distribution>> ListAsync(IEnvironment environment)
        {
            Ensure.NotNull(environment, nameof(environment));

            return db.Distributions.QueryAsync(
                And(Eq("environmentId", environment.Id), IsNull("deleted"))
            );
        }

        public async Task<Distribution> GetAsync(long id)
        {
            return await db.Distributions.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.Environment, id);
        }

        public async Task<Distribution> GetAsync(ResourceProvider provider, string resourceId)
        {
            Ensure.NotNull(provider, nameof(provider));

            return await db.Distributions.QueryFirstOrDefaultAsync(
                Conjunction(Eq("providerId", provider.Id), Eq("resourceId", resourceId))
            ) ?? throw ResourceError.NotFound(provider, ResourceTypes.Distribution, resourceId);
        }
     
        public async Task<Distribution> CreateAsync(CreateDistributionRequest request)
        {
            Ensure.NotNull(request, nameof(request));
            
            var distribution = new Distribution(
                id            : await db.Distributions.Sequence.NextAsync(),
                ownerId       : request.OwnerId,
                environmentId : request.EnvironmentId,
                providerId    : request.ProviderId,
                resourceId    : request.ResourceId,
                properties    : request.Properties
            );
            
            await db.Distributions.InsertAsync(distribution);

            return distribution;
        }

        public async Task ActivateAsync(Distribution record)
        {
            Ensure.NotNull(record, nameof(record));

            await db.Distributions.PatchAsync(record.Id, new[] {
                Change.Replace("activated", Func("NOW"))
            });
        }

        public async Task DeactivateAsync(Distribution record)
        {
            Ensure.NotNull(record, nameof(record));

            await db.Distributions.PatchAsync(record.Id, new[] {
                Change.Remove("activated")
            });
        }

        public async Task<bool> DeleteAsync(Distribution record)
        {
            Ensure.NotNull(record, nameof(record));
            
            return await db.Distributions.PatchAsync(record.Id, new[] {
                Change.Remove("activated"), // deactivate
                Change.Replace("deleted", Func("NOW")),
            }, condition: IsNull("deleted")) > 0;
        }
    }
}