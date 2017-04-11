namespace Carbon.Platform
{
    public enum ResourceType
    {
        // Identity & Access
        Account              = 100,
        Service              = 110,
        ServiceRole          = 111,
        User                 = 120,
        UserRole             = 121,
        
        // Environments
        Environment          = 200,
        Region               = 210,
        Zone                 = 211,

        // Computing | 100
        App                  = 300,
        AppRelease           = 302,
        Function             = 310, // An app with a entry point
        HealthCheck          = 320,
        Host                 = 330, // includes metal, vm instances, and container instances
        HostGroup            = 340,
        HostTemplate         = 350,
        MachineImage         = 360,
        MachineType          = 370,

        // Data & Storage (200)    
        Bucket               = 400,
        BucketObject         = 401,
        Database             = 410,
        DatabaseCluster      = 411,
        DatabaseInstance     = 412,
        EncryptionKey        = 420,
        Firehose             = 430,
        Queue                = 450,
        QueueMessage         = 451,
        Repository           = 460,
        Stream               = 470,
        StreamMessage        = 471,
        Volume               = 490,

        // Networking | 500
        Network              = 500,
        NetworkAddress       = 502, 
        NetworkGateway       = 503, 
        NetworkInterface     = 504,
        NetworkPeer          = 505,
        NetworkSecurityGroup = 510, // Rules are embeded
        LoadBalancer         = 520,
        NetworkProxyListener = 521,
        NetworkRouter        = 530,
        NetworkRoute         = 531,
        Subnet               = 580,

        // Domains & DNS     
        Certificate          = 600,
        Domain               = 610,
        DnsZone              = 620,
        DnsRecord            = 621,

        Website              = 700,
        WebsiteEnvironment   = 701,
        WebComponent         = 710,
        WebLibrary           = 720,
    
        // Metrics / Monitoring
        Metric              = 800
    }
}
