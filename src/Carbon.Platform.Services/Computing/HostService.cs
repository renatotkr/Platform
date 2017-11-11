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

        public async Task<HostInfo> GetAsync(string name)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            if (long.TryParse(name, out var id))
            {
                return await GetAsync(id);
            }

            var (provider, resourceId) = ResourceName.Parse(name);

            return await FindAsync(provider, resourceId) 
                ?? throw ResourceError.NotFound(ManagedResource.Host(provider, name));
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
            var location = LocationId.Create(request.Resource.LocationId);
            var regionId = location.WithZoneNumber(0);

            long machineTypeId = 0;

            if (request.MachineType != null)
            {
                if (request.MachineType.Id != null)
                {
                    machineTypeId = request.MachineType.Id.Value;
                }
                else
                {
                    switch (location.ProviderId)
                    {
                        case 2:
                            // AWS
                            machineTypeId = AwsInstanceType.Get(request.MachineType.Name).Id;
                            break;
                       
                        default:
                            var machineType = await GetMachineTypeAsync(location.ProviderId, request.MachineType.Name);

                            machineTypeId = machineType.Id;
                            break;
                    }   
                }
            }
            
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
                machineTypeId : machineTypeId,
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

        private async Task<MachineType> GetMachineTypeAsync(int providerId, string name)
        {
            var model = await db.MachineTypes.QueryFirstOrDefaultAsync(
                And(Eq("providerId", providerId), Eq("name", name))
            );

            if (model == null)
            {
                throw new ResourceNotFoundException(
                    resource: ManagedResource.MachineType(ResourceProvider.Get(providerId), name)
                );
            }

            return model;
        }

        private static readonly string nextIdSql = SqlHelper.GetCurrentValueAndIncrement<LocationInfo>("hostCount");

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