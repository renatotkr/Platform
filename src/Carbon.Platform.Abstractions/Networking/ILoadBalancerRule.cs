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

/*
path matches /images/* && host matches carbon.net
*/