using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Environments;
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
            return await db.Hosts.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.Host, id);
        }

        // e.g. 1 || aws:i-18342354, gcp:1234123123, azure:1234123, do:???

        public Task<HostInfo> GetAsync(string name)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            if (long.TryParse(name, out var id))
            {
                return GetAsync(id);
            }

            (var provider, var resourceId) = ResourceName.Parse(name);

            return FindAsync(provider, resourceId) 
                ?? throw ResourceError.NotFound(provider, ResourceTypes.Host, name);
        }

        public async Task<HostInfo> FindAsync(ResourceProvider provider, string resourceId)
        {
            return await db.Hosts.FindAsync(provider, resourceId);
        }

        public Task<IReadOnlyList<HostInfo>> ListAsync(long ownerId)
        {
            return db.Hosts.QueryAsync(
                expression : And(Eq("ownerId", ownerId), IsNull("terminated")),
                order      : Order.Descending("id"),
                take       : 1000
            );
        }

        public Task<IReadOnlyList<HostInfo>> ListAsync(ICluster cluster)
        {
            #region Preconditions

            if (cluster == null)
                throw new ArgumentNullException(nameof(cluster));

            #endregion

            return db.Hosts.QueryAsync(
                And(Eq("clusterId", cluster.Id), IsNull("terminated"))
            );
        }

        public Task<IReadOnlyList<HostInfo>> ListAsync(IEnvironment environment)
        {
            #region Preconditions

            if (environment == null)
                throw new ArgumentNullException(nameof(environment));

            #endregion

            return db.Hosts.QueryAsync(
                And(Eq("environmentId", environment.Id), IsNull("terminated"))
            );
        }

        public async Task<HostInfo> RegisterAsync(RegisterHostRequest request)
        {
            var regionId = LocationId.Create(request.Resource.LocationId).WithZoneNumber(0);

            var host = new HostInfo(
                id            : await GetNextId(regionId),
                type          : request.Type,
                status        : request.Status,
                addresses     : request.Addresses,
                clusterId     : request.ClusterId,
                imageId       : request.ImageId,
                locationId    : request.LocationId,
                resource      : request.Resource,
                environmentId : request.EnvironmentId,
                machineTypeId : request.MachineTypeId,
                ownerId       : request.OwnerId
            );
        
            await db.Hosts.InsertAsync(host);

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

        static readonly string nextIdSql = SqlHelper.GetCurrentValueAndIncrement<LocationInfo>("hostCount");

        // 4B per zone per region
        private async Task<HostId> GetNextId(LocationId locationId)
        {
            #region Preconditions

            if (locationId.Value == 0)
                throw new ArgumentException("Must not be 0", nameof(locationId));

            #endregion

            ILocation location = await db.Locations.FindAsync(locationId.Value);

            // Ensure the location exists
            if (location == null)
            {
                location = Locations.Get(locationId);

                await db.Locations.InsertAsync(new LocationInfo(location.Id, location.Name));
            }

            int currentHostCount;

            using (var connection = await db.Context.GetConnectionAsync())
            {
                currentHostCount = await connection.ExecuteScalarAsync<int>(nextIdSql, location);
            }

            return HostId.Create(locationId, currentHostCount + 1);
        }     
    }
}