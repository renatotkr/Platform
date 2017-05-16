using System;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    using static Expression;

    public class HostGroupService : IHostGroupService
    {
        private readonly PlatformDb db;

        public HostGroupService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        public async Task<HostGroup> CreateAsync(CreateHostGroupRequest request)
        {
            #region Preconditions

            if (LocationId.Create(request.Location.Id).ZoneNumber > 0)
                throw new ArgumentException("Must be a region. Was a zone.", nameof(request.Location));

            // TODO: Add support for zone groups

            #endregion
            
            // e.g. carbon/us-east-1

            var group = new HostGroup(
               id            : await db.HostGroups.GetNextScopedIdAsync(request.Environment.Id).ConfigureAwait(false),
               name          : request.Environment.Name + "/" + request.Location.Name,
               environmentId : request.Environment.Id,
               resource      : ManagedResource.HostGroup(request.Location, Guid.NewGuid().ToString())
            );

            group.Details = request.Details;

            await db.HostGroups.InsertAsync(group).ConfigureAwait(false);

            return group;
        }

        public async Task<HostGroup> GetAsync(long id)
        {
            return await db.HostGroups.FindAsync(id) ?? throw ResourceError.NotFound(ResourceType.HostGroup, id);
        }

        public async Task<HostGroup> GetAsync(IEnvironment env, ILocation location)
        {
            var group = await db.HostGroups.QueryFirstOrDefaultAsync(
                Conjunction(
                    Eq("environmentId", env.Id),
                    Eq("locationId", location.Id),
                    IsNull("deleted")
                )
            ).ConfigureAwait(false);

            if (group == null)
            {
                throw new ResourceNotFoundException($"hostGroup(env#{env.Id}, location#{location.Id})");
            }
            
            return group;
        }
    }
}