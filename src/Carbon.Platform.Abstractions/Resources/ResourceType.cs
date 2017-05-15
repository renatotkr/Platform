namespace Carbon.Platform.Resources
{
    public enum ResourceType
    {
        Entity              = 1, // Organization, Person, or IA
        Environment         = 2, // An environment actions take place within
        Account             = 3,
        User                = 4, // Person or service interacting with the system (note: all users have an account)
        Session             = 5, // An authenticated scope for a user       

        Location            = 10,

        // COMPUTING  -------------------------------------------------------------------------------------
        App                 = 30, // AKA Program
        Host                = 40, // includes metal, vm instances, and container instances
        HostGroup           = 41, // A group of hosts with a specific configuration
        HostTemplate        = 42,
        MachineImage        = 50,
        MachineType         = 51,

        // Storage --------------------------------------------------------------------------------------------
        
        Bucket              = 100,        
        Channel             = 110, // AKA Stream / Topic           
        Database            = 120,
        DatabaseCluster     = 121,
        DatabaseInstance    = 122,
        DatabaseSchema      = 123,
        DataEncryptionKey   = 131, // AKA dek
        EncryptionKey       = 130, // master encryption keys
        Queue               = 140,         
        Repository          = 150,
        RepositoryCommit    = 151,             
        Volume              = 160, // AKA drive

        // Hosting --------------------------------------------------------------------------------------------

        Network              = 200,
        Subnet               = 201,
        NetworkAddress       = 202, 
        NetworkInterface     = 204,

        // - Load Balancers
        LoadBalancer         = 210,
        LoadBalancerListener = 211,
        LoadBalancerRule     = 212,

        // - Routing ---
        NetworkPeer          = 221,
        NetworkRouter        = 222,
        NetworkRoute         = 223,

        // - Security ---
        NetworkSecurityGroup = 230,

        // Hosting --------------------------------------------------------------------------------------------

        Domain               = 300,
        Certificate          = 301,
                             
        // - DNS -           
        DnsZone              = 310,
        DnsRecord            = 311,
                             
        Website              = 340,
        WebComponent         = 341,
        WebLibrary           = 342,
                             
        HealthCheck          = 360,

        // CI ------------------------------------------------------------------------------------------------
        Deployment           = 400,

        // Metrics --------------------------------------------------------------------------------------------
        Metric               = 500        
    }
}