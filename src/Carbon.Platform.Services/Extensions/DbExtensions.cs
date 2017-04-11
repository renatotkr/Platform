using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;

namespace Carbon.Platform
{
    using Apps;
    using Computing;
    using Networking;

    using static Expression;

    public static class DbExtensions
    {
        // Networking

        public static Task<NetworkInfo> FindAsync(this Dataset<NetworkInfo, long> dataset, ResourceProvider provider, string id) =>
           dataset.QueryFirstOrDefaultAsync(And(Eq("providerId", provider.Id), Eq("resourceId", id)));

        public static Task<NetworkInterfaceInfo> FindAsync(this Dataset<NetworkInterfaceInfo, long> dataset, ResourceProvider provider, string id) =>
            dataset.QueryFirstOrDefaultAsync(And(Eq("providerId", provider.Id), Eq("resourceId", id)));

        public static Task<SubnetInfo> FindAsync(this Dataset<SubnetInfo, long> dataset, ResourceProvider provider, string id) =>
            dataset.QueryFirstOrDefaultAsync(And(Eq("providerId", provider.Id), Eq("resourceId", id)));

        public static Task<NetworkSecurityGroup> FindAsync(this Dataset<NetworkSecurityGroup, long> dataset, ResourceProvider provider, string id) =>
            dataset.QueryFirstOrDefaultAsync(And(Eq("providerId", provider.Id), Eq("resourceId", id)));

        public static Task<LoadBalancer> FindAsync(this Dataset<LoadBalancer, long> dataset, ResourceProvider provider, string id) =>
           dataset.QueryFirstOrDefaultAsync(And(Eq("providerId", provider.Id), Eq("resourceId", id)));

        // ----

        // Computing

        public static Task<HostGroup> FindAsync(this Dataset<HostGroup, long> dataset, ResourceProvider provider, string id) =>
            dataset.QueryFirstOrDefaultAsync(And(Eq("providerId", provider.Id), Eq("resourceId", id)));

        public static Task<HostInfo> FindAsync(this Dataset<HostInfo, long> dataset, ResourceProvider provider, string id) =>
            dataset.QueryFirstOrDefaultAsync(And(Eq("providerId", provider.Id), Eq("resourceId", id)));

        public static Task<MachineImageInfo> FindAsync(this Dataset<MachineImageInfo, long> dataset, ResourceProvider provider, string id) =>
          dataset.QueryFirstOrDefaultAsync(And(Eq("providerId", provider.Id), Eq("resourceId", id)));

        public static Task<VolumeInfo> FindAsync(this Dataset<VolumeInfo, long> dataset, ResourceProvider provider, string id) =>
          dataset.QueryFirstOrDefaultAsync(And(Eq("providerId", provider.Id), Eq("resourceId", id)));

        // ----

        public static Task<IReadOnlyList<HostInfo>> FindHavingApp(this Dataset<HostInfo, long> dataset, IApp app) => 
            dataset.QueryAsync(Eq("appId", app.Id));

        public static Task<IReadOnlyList<AppRelease>> FindHavingApp(this Dataset<AppRelease, long> dataset, IApp app) => 
            dataset.QueryAsync(Eq("appId", app.Id), Order.Descending("version"));
    }
}