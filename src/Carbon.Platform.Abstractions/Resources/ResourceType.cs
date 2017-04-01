namespace Carbon.Platform
{
    public enum ResourceType
    {
        // Identity & Access
        Account             = 1, 
        User                = 2,
        ServiceRole         = 3,

        // Locations
        Location            = 10,
        Region              = 11,
        Zone                = 12,

        // Computing | 100
        App                 = 100,
        AppRelease          = 101,
        Function            = 102, // An app with a single entry point
        Backend             = 110,
        HealthCheck         = 111,
        Host                = 120, // includes metal, vm instances, and container instances
        HostTemplate        = 121,
        MachineImage        = 150,
        MachineType         = 151,

        // Data & Storage (200)        
        Blob                = 200,
        Bucket              = 201,
        Database            = 210,
        EncryptionKey       = 220,
        Repository          = 230,
        RepositoryBlob      = 231,
        RepositoryObject    = 232,
        RepositoryRevision  = 233,
        Stream              = 240,
        Volume              = 250,
        Queue               = 260,

        // Domains & DNS
        Certificate         = 300,
        Domain              = 310,
        DnsZone             = 311,
        DnsRecord           = 312,

        // Networking | 500
        Network              = 500,
        NetworkAddress       = 502, 
        NetworkGateway       = 503, 
        NetworkInterface     = 504,
        NetworkPeer          = 505,
        NetworkPolicy        = 510,
        NetworkPolicyRule    = 511,
        NetworkProxy         = 520,
        NetworkProxyListener = 521,
        NetworkRoute         = 530,
        NetworkRouter        = 531,
        Subnet               = 540,
    }
}
