using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public interface ILoadBalancer : IManagedResource
    {
        // gcp | Unicast IP Address
        // aws | CNAME (name-424835706.us-west-2.elb.amazonaws.com)
        string Address { get; }
    }
}

// A load balancer may service mutiple hosts / environments using rules
// aws: arn:aws:elasticloadbalancing:us-west-2:123456789012:loadbalancer/app/my-load-balancer/50dc6c495c0c9188