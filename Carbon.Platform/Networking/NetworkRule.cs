namespace Carbon.Networking
{
    using Data.Annotations;

    // AKA firewall rule

    [Record(TableName = "NetworkRules")]
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
        Allow,
        Deny
    }   
}
