namespace Carbon.Platform.Networking
{
    using Data.Annotations;

    [Dataset("NetworkPeers")]
    public class NetworkPeer
    {
        [Member(1)]
        public long Id { get; set; }
 
        public long NetworkId => ScopedId.GetScope(Id);
    }
}