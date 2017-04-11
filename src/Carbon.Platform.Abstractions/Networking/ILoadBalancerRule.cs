namespace Carbon.Platform.Networking
{
    public interface ILoadBalancerRule
    {
        long Id { get; }

        long LoadBalancerId { get; }

        string Condition { get; }

        string Action { get; }

        int Priority { get; }
    }
}


/*
path matches /images/*
host matches carbon.net
*/