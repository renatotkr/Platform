using Carbon.Net;

namespace Carbon.Platform.Networking
{
    public interface IFirewallRule
    {
        string Name { get; }

        //  6 (TCP) 17 (UDP), and 1 (ICMP).

        NetworkProtocal Protocal { get; }    // e.g. tpc *
        
        TrafficDirection Direction { get; }  // e.g. in | out

        string Source { get; }

        string SourcePorts { get; }          // e.g. 80, 7000-8000

        /// <summary>
        /// A list of comma seperated tokens
        /// Addresses can include security groups, ip addresses, cidr blocks, or *
        /// e.g. 10.0.0.0/24,sg-104324234
        /// </summary>
        string Destination { get; }          // e.g. *

        string DestinationPorts { get; }

        FirewallRuleAction Action { get; }   // allow, deny [block?]

        int Priority { get; }
    }
}

// cidr, group, ...
// e.g. 0.0.0.0/0	

// sourcePort = 8000-8010
// source     = 0.0.0.0/0

// targetPort = 22

// Enforced by network devices (proxies, switches, etc)

// Apart of proxy configuration, network security groups
/*
CONDITIONS ---------------------------

direction = in | out
protocal = tcp | upc
port range = 80 | 100-200
ip matches 0.0.0.0/0 
group matches sg-11aa22b

ACTIONS ------------------------------
forward backend#100
allow
drop
log

*/



/* 
Rule #  Protocal	Source	          Port	    Action      Priority
100	    TCP         0.0.0.0/0	      3389	    allow       1
101     UPC         0.0.0.0/9         1000      deny        2
102     *                                                   3
*/

/*
protocol 
sourcePortRange 
destinationPortRange  
sourceAddressPrefix
destinationAddressPrefix 
direction  
priority  
access 
*/