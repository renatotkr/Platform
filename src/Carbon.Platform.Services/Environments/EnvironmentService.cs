using System;
using System.Threading.Tasks;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        private readonly PlatformDb db;

        public EnvironmentService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<EnvironmentInfo> GetAsync(long id)
        {
            return await db.Environments.FindAsync(id).ConfigureAwait(false)
                ?? throw ResourceError.NotFound(ResourceTypes.Environment, id);
        }

        public Task<EnvironmentInfo> GetAsync(long programId, EnvironmentType type)
        {
            // We can lookup directly by id...

            long id = programId + (((int)type) - 1);

            return GetAsync(id);
        }
    }
}