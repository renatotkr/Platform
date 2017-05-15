using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Computing;
using Carbon.Platform.Resources;
using Carbon.Platform.Networking;

using Dapper;

namespace Carbon.Platform.Services
{
    using static Expression;

    public class HostService : IHostService
    {
        private readonly PlatformDb db;

        public HostService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        // e.g. 1 || aws:i-18342354, gcp:1234123123, azure:1234123

        public Task<HostInfo> GetAsync(string idOrResourceId)
        {
            if (long.TryParse(idOrResourceId, out var id))
            {
                return GetAsync(id);
            }
            else
            {
                var parts = idOrResourceId.Split(':');

                var provider = ResourceProvider.Parse(parts[0]);
                var resourceId = parts[1];

                return FindAsync(provider, resourceId) ?? throw new Exception($"{provider.Code}:host/{id} not found");
            }
        }

        public async Task<HostInfo> GetAsync(long id)
        {
            return await db.Hosts.FindAsync(id).ConfigureAwait(false) ?? throw new Exception($"host#{id} not found");
        }

        public async Task<HostInfo> FindAsync(ResourceProvider provider, string id)
        {
            return await db.Hosts.FindAsync(provider, id).ConfigureAwait(false);
        }

        public async Task<HostInfo> CreateAsync(CreateHostRequest request)
        {
             var host = new HostInfo(
                id             : await GetNextId(request.Location).ConfigureAwait(false),
                type           : HostType.Virtual,
                status         : request.Status,
                addresses      : request.Addresses,
                resource       : request.Resource,
                environmentId  : request.EnvironmentId,
                machineTypeId  : request.MachineTypeId,
                machineImageId : request.MachineImageId,
                networkId      : request.NetworkId,
                created        : DateTime.UtcNow
            );

            host.GroupId = request.GroupId;
        
            await db.Hosts.InsertAsync(host).ConfigureAwait(false);

            return host;
        }

        #region Groups

        public async Task<HostGroup> CreateHostGroupAsync(IEnvironment env, ILocation region)
        {
            #region Preconditions

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            if (region == null)
                throw new ArgumentNullException(nameof(region));

            if (LocationId.Create(region.Id).ZoneNumber > 0)
                throw new ArgumentException("Must be a region. Was a zone.", nameof(region));

            // TODO: Add support for zone groups

            #endregion
            
            // e.g. carbon/us-east-1

            var group = new HostGroup(
               id            : await db.HostGroups.GetNextScopedIdAsync(env.Id).ConfigureAwait(false),
               name          : env.Name + "/" + region.Name,
               environmentId : env.Id,
               resource      : ManagedResource.HostGroup(region, Guid.NewGuid().ToString())
            );

            await db.HostGroups.InsertAsync(group).ConfigureAwait(false);

            return group;
        }

        public async Task<HostGroup> GetGroupAsync(long id)
        {
            return await db.HostGroups.FindAsync(id) ?? throw new Exception($"hostGroup#{id} not found");
        }

        public async Task<HostGroup> GetGroupAsync(IEnvironment env, ILocation location)
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
                throw new ArgumentException($"hostGroup with env#{env.Id} and location#{location.Id} not found");
            }
            
            return group;
        }

        #endregion

        #region Network Interfaces

        public Task<IReadOnlyList<NetworkInterfaceInfo>> GetNetworkInterfacesAsync(long hostId)
        {
            return db.NetworkInterfaces.QueryAsync(Eq("hostId", hostId));
        }

        #endregion

        #region Volumes

        public Task<IReadOnlyList<VolumeInfo>> GetVolumesAsync(long hostId)
        {
            return db.Volumes.QueryAsync(Eq("hostId", hostId));
        }

        #endregion

        public async Task<MachineImageInfo> GetMachineImageAsync(ResourceProvider provider, string id)
        {
            var image = await db.MachineImages.FindAsync(provider, id).ConfigureAwait(false);

            if (image == null)
            {
                // var ami = await ec2.DescribeImageAsync(amiId);
                
                image = new MachineImageInfo(
                    id       : db.MachineImages.Sequence.Next(),
                    type     : MachineImageType.Machine,
                    name     : Guid.NewGuid().ToString(),
                    resource : new ManagedResource(ResourceProvider.Aws, ResourceType.MachineImage, id)
                );

                await db.MachineImages.InsertAsync(image).ConfigureAwait(false);
            }

            return image;
        }

        // 4B per zone per region
        private async Task<HostId> GetNextId(ILocation location)
        {
            // Ensure the location exists
            if (await db.Locations.FindAsync(location.Id).ConfigureAwait(false) == null)
            {
                await db.Locations.InsertAsync(new LocationInfo(location.Id, location.Name)).ConfigureAwait(false);
            }

            int sequenceNumber;

            using (var connection = db.Context.GetConnection())
            {
                sequenceNumber = await connection.ExecuteScalarAsync<int>(
                    @"SELECT `hostCount` FROM `Locations` WHERE id = @id FOR UPDATE;
                    UPDATE `Locations`
                    SET `hostCount` = `hostCount` + 1
                    WHERE id = @id", location).ConfigureAwait(false) + 1;
            }

            return HostId.Create(location, sequenceNumber);
        }     
    }
}