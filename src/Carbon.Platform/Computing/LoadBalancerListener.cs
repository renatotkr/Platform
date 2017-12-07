using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Net;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Computing
{
    [Dataset("LoadBalancerListener", Schema = "Computing")]
    [UniqueIndex("providerId", "resourceId")]
    public class LoadBalancerListener : ILoadBalancerListener
    {
        public LoadBalancerListener() { }

        public LoadBalancerListener(
            long id, 
            ApplicationProtocal protocal,
            ushort port,
            ManagedResource resource,
            JsonObject properties)
        {
            Validate.Id(id);

            Id         = id;
            Protocal   = protocal;
            Port       = port;
            ProviderId = resource.ProviderId;
            ResourceId = resource.ResourceId;
            LocationId = resource.LocationId;
            Properties = properties; 
        }

        // loadBalancerId | #
        [Member("id"), Key]
        public long Id { get;  }

        // e.g. HTTP | HTTPS
        [Member("protocal")]
        public ApplicationProtocal Protocal { get; }

        [Member("port")]
        public ushort Port { get; }

        // TODO: CertificateId

        public long LoadBalancerId => ScopedId.GetScope(Id);

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; set; }

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; }
        
        [Member("resourceId")]
        [Ascii, StringLength(120)]
        public string ResourceId { get; }

        [Member("locationId")]
        public int LocationId { get; }

        // arn:aws:elasticloadbalancing:us-west-2:123456789012:listener/app/my-load-balancer/50dc6c495c0c9188/f2f7dc8efc522ab2

        ResourceType IResource.ResourceType => ResourceTypes.LoadBalancerListener;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }
      
        #endregion
    }
}