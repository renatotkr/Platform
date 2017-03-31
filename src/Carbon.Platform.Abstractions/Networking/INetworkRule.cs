namespace Carbon.Platform.Networking
{
    public interface INetworkRule
    {        
        /*
        string SourcePort    { get; }
        string SourceAddress { get; }    // cidr | group 

        string TargetPort    { get; }
        string TargetAddress { get; }    // cidr | group
        */

        string Condition     { get; }

        string Action        { get; }

        int Priority { get; }
    }

    // sourcePort    = 8000-8010
    // sourceAddress =  0.0.0.0/0

    // targetPort = 22

    public enum TrafficDirection : byte
    {
        Inbound  = 1, // ingree
        Outbound = 2  // egress
    }


    // Protocal
    // Source Port(s)
    // Destination Port(s)
    // Source Address (may be Cidr range)
    // Destination Address (may be Cidr range)
    // Action (Allow | Deny)
    // Direction 

    
    // Enforced by network devices (proxies, switches, etc)
    // Apart of proxy configuration, network security groups
}

/*
CONDITIONS ---------------------------

direction = inbound
protocal = tcp | upc
source port = 80
target port = 100-200
ip matches 0.0.0.0/0 
group matches sg-11aa22b

source matches 0.0.0.0, 10.10.0.0/24
path matches /images/*
host matches carbon.net

ACTIONS ------------------------------
forward backend#100
allow
drop
log

*/
