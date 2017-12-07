using System;
using System.Threading.Tasks;

using Amazon.Elb;

using Carbon.Cloud.Logging;
using Carbon.Platform.Computing;

namespace Carbon.Platform.Management
{
    public sealed class ClusterManager : IClusterManager
    {
        private readonly ElbClient elbClient;
        private readonly IEventLogger eventLog;
        private readonly IClusterService clusterService;

        public ClusterManager(IClusterService clusterService, ElbClient elbClient, IEventLogger eventLog)
        {
            this.clusterService = clusterService ?? throw new ArgumentNullException(nameof(clusterService));
            this.elbClient      = elbClient      ?? throw new ArgumentNullException(nameof(elbClient));
            this.eventLog       = eventLog       ?? throw new ArgumentNullException(nameof(eventLog));
        }

        public async Task RegisterHostAsync(Cluster cluster, HostInfo host)
        {
            Validate.NotNull(cluster, nameof(cluster));
            Validate.NotNull(host,    nameof(host));
            
            // - Register to any attached load balancers

            if (cluster.Properties.TryGetValue(ClusterProperties.TargetGroupArn, out var targetGroupArnNode))
            {
                var registration = new RegisterTargetsRequest(
                    targetGroupArn : targetGroupArnNode,
                    targets        : new[] { new TargetDescription(id: host.ResourceId) }
                );

                // Register the instances with the lb's target group
                await elbClient.RegisterTargetsAsync(registration);

                await eventLog.CreateAsync(new Event(
                    action   : "c",
                    resource : "borg:host/" + host.Id
                ));
            }
        }

        public async Task DeregisterHostAsync(Cluster cluster, HostInfo host)
        {
            Validate.NotNull(cluster, nameof(cluster));
            Validate.NotNull(host, nameof(host));

            // - Deregister from any attached load balancers

            if (cluster.Properties != null &&
                   cluster.Properties.TryGetValue(ClusterProperties.TargetGroupArn, out var targetGroupArn))
            {
                await elbClient.DeregisterTargetsAsync(new DeregisterTargetsRequest(
                    targetGroupArn: targetGroupArn,
                    targets: new[] {
                        new TargetDescription(host.ResourceId)
                    }
                ));

                // cluster / remove...
                await eventLog.CreateAsync(new Event(
                    action  : "drain",
                    resource: "borg:host/" + host.Id
                ));
            }
        }
    }
}