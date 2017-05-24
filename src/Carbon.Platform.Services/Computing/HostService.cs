using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Networking;
using Carbon.Platform.Resources;
using Carbon.Platform.Services;
using Carbon.Platform.Storage;

using Dapper;

namespace Carbon.Platform.Computing
{
    using static Expression;

    public class HostService : IHostService
    {
        private readonly PlatformDb db;

        public HostService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<HostInfo> GetAsync(long id)
        {
            return await db.Hosts.FindAsync(id).ConfigureAwait(false)
                ?? throw ResourceError.NotFound(ResourceTypes.Host, id);
        }

        // e.g. 1 || aws:i-18342354, gcp:1234123123, azure:1234123

        public Task<HostInfo> GetAsync(string name)
        {
            if (long.TryParse(name, out var id))
            {
                return GetAsync(id);
            }

            (var provider, var resourceId) = ResourceName.Parse(name);

            return FindAsync(provider, resourceId) ?? throw ResourceError.NotFound(provider, ResourceTypes.Host, name);
        }

        public async Task<HostInfo> FindAsync(ResourceProvider provider, string id)
        {
            return await db.Hosts.FindAsync(provider, id).ConfigureAwait(false);
        }
        
        public async Task<HostInfo> RegisterAsync(RegisterHostRequest request)
        {
             var host = new HostInfo(
                id            : await GetNextId(request.Location).ConfigureAwait(false),
                type          : HostType.Virtual,
                status        : request.Status,
                addresses     : request.Addresses,
                clusterId     : request.ClusterId,
                resource      : request.Resource,
                environmentId : request.EnvironmentId,
                machineTypeId : request.MachineTypeId,
                imageId       : request.ImageId,
                networkId     : request.NetworkId,
                ownerId       : request.OwnerId,
                created       : DateTime.UtcNow
            );
        
            await db.Hosts.InsertAsync(host).ConfigureAwait(false);

            return host;
        }

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