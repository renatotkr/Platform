using System;
using System.Linq;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Computing;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Services
{
    using static Expression;

    public class EnvironmentService : IEnvironmentService
    {
        private readonly PlatformDb db;

        public EnvironmentService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<EnvironmentInfo> GetAsync(long id)
        {
            return db.Environments.FindAsync(id) ?? throw ResourceError.NotFound(ResourceTypes.Environment, id);
        }

        public Task<EnvironmentInfo> GetAsync(long programId, EnvironmentType type)
        {
            // We can lookup directly by id...

            long id = programId + (((int)type) - 1);

            return GetAsync(id);
        }

        public async Task<IHost[]> GetHostsAsync(IEnvironment env, ILocation location)
        {
            var locationId = LocationId.Create(location.Id);

            var idStart = HostId.Create(locationId.WithZoneNumber(0),             sequenceNumber: 0);
            var idEnd   = HostId.Create(locationId.WithZoneNumber(byte.MaxValue), sequenceNumber: int.MaxValue);

            var hosts = await db.Hosts.QueryAsync(
                Conjunction(
                    Between("id", idStart, idEnd),
                    Eq("environmentId", env.Id),
                    IsNull("terminated")
                )
            ).ConfigureAwait(false);

            return hosts.ToArray();
        }
    }
}