using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            return FindAsync(provider, resourceId) 
                ?? throw ResourceError.NotFound(provider, ResourceTypes.Host, name);
        }

        public async Task<HostInfo> FindAsync(ResourceProvider provider, string id)
        {
            return await db.Hosts.FindAsync(provider, id).ConfigureAwait(false);
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

        public Task<IReadOnlyList<HostInfo>> ListAsync(IEnvironment environment, ILocation location)
        {
            var locationId = LocationId.Create(location.Id);

            var idStart = HostId.Create(locationId.WithZoneNumber(0), sequenceNumber: 0);
            var idEnd   = HostId.Create(locationId.WithZoneNumber(byte.MaxValue), sequenceNumber: int.MaxValue);

            return db.Hosts.QueryAsync(
                Conjunction(
                    Eq("environmentId", environment.Id), // env index...
                    Between("id", idStart, idEnd),
                    IsNull("terminated")
                )
            );
        }

        public async Task<HostInfo> RegisterAsync(RegisterHostRequest request)
        {
            var regionId = LocationId.Create(request.Resource.LocationId).WithZoneNumber(0);

            var host = new HostInfo(
                id            : await GetNextId(regionId).ConfigureAwait(false),
                type          : request.Type,
                status        : request.Status,
                addresses     : request.Addresses,
                clusterId     : request.ClusterId,
                imageId       : request.ImageId,
                programId     : request.ProgramId,
                locationId    : request.LocationId,
                resource      : request.Resource,
                environmentId : request.EnvironmentId,
                machineTypeId : request.MachineTypeId,
                networkId     : request.NetworkId,
                ownerId       : request.OwnerId
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

        static readonly string nextIdSql = SqlHelper.GetCurrentValueAndIncrement<LocationInfo>("hostCount");

        // 4B per zone per region
        private async Task<HostId> GetNextId(LocationId locationId)
        {
            #region Preconditions

            if (locationId.Value == 0)
                throw new ArgumentException("Must not be 0", nameof(locationId));

            #endregion

            ILocation location = await db.Locations.FindAsync(locationId.Value).ConfigureAwait(false);

            // Ensure the location exists
            if (location == null)
            {
                location = Locations.Get(locationId);

                await db.Locations.InsertAsync(new LocationInfo(location.Id, location.Name)).ConfigureAwait(false);
            }

            int currentHostCount;

            using (var connection = await db.Context.GetConnectionAsync())
            {
                currentHostCount = await connection.ExecuteScalarAsync<int>(nextIdSql, location).ConfigureAwait(false);
            }

            return HostId.Create(locationId, currentHostCount + 1);
        }     
    }
}