using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public interface ILoadBalancerRule : IResource
    {
        long LoadBalancerId { get; }

        string Condition { get; }

        string Action { get; }

        int Priority { get; }
    }
}

// path matches /images/* && host matches carbon.net
// forward -> hostGroup#100
// arn:aws:elasticloadbalancing:ua-west-2:123456789012:targetgroup/my-targets/73e2d6bc24d8a067