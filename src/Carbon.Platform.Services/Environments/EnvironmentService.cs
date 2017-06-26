using System;
using System.Threading.Tasks;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Environments
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

        public async Task<EnvironmentInfo> CreateAsync(CreateEnvironmentRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

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