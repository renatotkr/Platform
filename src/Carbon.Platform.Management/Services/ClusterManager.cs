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
            #region Preconditions

            if (cluster == null)
                throw new ArgumentNullException(nameof(cluster));

            if (host == null)
                throw new ArgumentNullException(nameof(host));

            #endregion
            
            // - Register to any attached load balancers

            if (cluster.Properties.ContainsKey(ClusterProperties.TargetGroupArn))
            {
                var registration = new RegisterTargetsRequest(
                    targetGroupArn  : cluster.Properties[ClusterProperties.TargetGroupArn],
                    targets         : new[] { new TargetDescription(id: host.ResourceId) }
                );

                // Register the instances with the lb's target group
                await elbClient.RegisterTargetsAsync(registration);

                await eventLog.CreateAsync(new Event(
                    action: "addToLoadBalancerTargetGroup",
                    resource: "host#" + host.Id
                ));
            }
        }

        public async Task DeregisterHostAsync(Cluster cluster, HostInfo host)
        {
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
                    resource: "host#" + host.Id
                ));
            }
        }
    }
}