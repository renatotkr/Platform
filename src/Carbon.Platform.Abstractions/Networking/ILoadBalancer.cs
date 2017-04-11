namespace Carbon.Platform.Networking
{
    public interface ILoadBalancer : IManagedResource
    {
        long Id { get; }

        // google : Unicast IP Address
        // aws    : CNAME (name-424835706.us-west-2.elb.amazonaws.com)
        string Address { get; }

        long EnvironmentId { get; }
    }
}

// AWS: arn:aws:elasticloadbalancing:us-west-2:123456789012:loadbalancer/app/my-load-balancer/50dc6c495c0c9188