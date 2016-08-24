namespace Carbon.Networking
{
    using Data.Annotations;

    [Record(TableName = "NetworkPeers")]
    public class NetworkPeer
    {
        [Member(1), Key]
        public long NetworkId { get; set; }

        [Member(2), Key] 
        public long Id { get; set; }
    }
}