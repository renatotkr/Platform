namespace Carbon.Networking
{
    using Data.Annotations;

    // AKA firewall rule

    [Dataset("Rules", Schema = "Networking")]
    public class NetworkRule
    {
        [Member(1), Key]
        public long NetworkId { get; set; }

        [Member(2), Key]
        public byte[] Hash { get; set; } // Hash of the rule

        [Member(3), Key]
        public IPAddressRange Target { get; set; }

        [Member(4), Key]
        public NetworkPortList Ports { get; set; }

        [Member(5), Key]
        public MatchAction Action { get; set; }
    }

    public enum MatchAction
    {
        Allow = 1,
        Deny  = 2,
        Log   = 3
    }   
}


// Google Cloud Notes:

// By default, all incoming traffic from outside a network is blocked and no packet is allowed into an instance without an appropriate firewall rule.
// Firewall rules only regulate incoming traffic to an instance.
// Once a connection has been established with an instance, traffic is permitted in both directions over that connection. 
// All instances are configured with a "hidden" firewall rule that drops TCP connections after 10 minutes of inactivity. 
