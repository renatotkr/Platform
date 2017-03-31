using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Expressions;

namespace Carbon.Platform
{
    using Apps;
    using Computing;
    using Data;
    using Networking;
    using Storage;

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

        public static Task<NetworkPolicy> FindAsync(this Dataset<NetworkPolicy, long> dataset, ResourceProvider provider, string id) =>
            dataset.QueryFirstOrDefaultAsync(And(Eq("providerId", provider.Id), Eq("resourceId", id)));

        public static Task<NetworkProxy> FindAsync(this Dataset<NetworkProxy, long> dataset, ResourceProvider provider, string id) =>
           dataset.QueryFirstOrDefaultAsync(And(Eq("providerId", provider.Id), Eq("resourceId", id)));

        // ----

        // Computing

        public static Task<BackendInfo> FindAsync(this Dataset<BackendInfo, long> dataset, ResourceProvider provider, string id) =>
            dataset.QueryFirstOrDefaultAsync(And(Eq("providerId", provider.Id), Eq("resourceId", id)));

        public static Task<HostInfo> FindAsync(this Dataset<HostInfo, long> dataset, ResourceProvider provider, string id) =>
            dataset.QueryFirstOrDefaultAsync(And(Eq("providerId", provider.Id), Eq("resourceId", id)));

        public static Task<ImageInfo> FindAsync(this Dataset<ImageInfo, long> dataset, ResourceProvider provider, string id) =>
          dataset.QueryFirstOrDefaultAsync(And(Eq("providerId", provider.Id), Eq("resourceId", id)));

        public static Task<VolumeInfo> FindAsync(this Dataset<VolumeInfo, long> dataset, ResourceProvider provider, string id) =>
          dataset.QueryFirstOrDefaultAsync(And(Eq("providerId", provider.Id), Eq("resourceId", id)));

        // ----

        public static Task<IReadOnlyList<AppInstance>> FindHavingApp(this Dataset<AppInstance, long> dataset, IApp app) => 
            dataset.QueryAsync(Eq("appId", app.Id));

        public static Task<IReadOnlyList<AppRelease>> FindHavingApp(this Dataset<AppRelease, long> dataset, IApp app) => 
            dataset.QueryAsync(Eq("appId", app.Id), Order.Descending("version"));
    }
}