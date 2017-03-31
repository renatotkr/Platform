using System;

namespace Carbon.Platform
{
    using static ResourceType;

    public enum ResourceType
    {
        // Identity & Access
        Account             = 1, 
        User                = 2,
        Role                = 3,

        // Apps         
        App                 = 10,
        AppRelease          = 11,
        Function            = 12, // An app with a single entry point
        
        // Compute
        Host                = 20, // includes metal, vm instances, and container instances
        HostTemplate        = 21,
        MachineImage        = 22,
        MachineType         = 23,

        // Scaling
        Backend             = 30,
        HealthCheck         = 31,

        // Storage          
        Bucket              = 40,
        Volume              = 41,
        Repository          = 42,

        Stream              = 43,
        Queue               = 44,

        // Databases
        Database            = 50,
        DatabaseCluster     = 51,
        DatabaseInstance    = 52,
        DatabaseBackup      = 53,

        // Locations
        Zone                 = 60, // AKA AvailabilityZone -- aws:zone:us-east-1b
        Region               = 61,
        Location             = 62,

        // Hosting           
        Domain               = 70,
        DnsZone              = 71,
        Certificate          = 72,
                             
        // Networking ( 100 - 200)
        Network              = 100, // AKA VPC
        NetworkAcl           = 101,
        NetworkAddress       = 102, // AKA IPAddress
        NetworkGateway       = 103, 
        NetworkInterface     = 104,
        NetworkPeer          = 105,
        NetworkProxy         = 106,
        NetworkProxyListener = 107,
        NetworkRule          = 108, // Enforced by network or proxy...
        NetworkRoute         = 109,
        Subnet               = 110, // AKA subnetwork / Network Partition

        // Security
        EncryptionKey       = 300
    }

    public static class ResourceTypeHelper
    {
        public static ResourceType Parse(string text)
        {
            switch (text.ToLower())
            {
                case "app"              : return App;
                case "bucket"           : return Bucket;
                case "domain"           : return Domain;
                case "encryptionkey"    : return EncryptionKey;
                case "function"         : return Function;

                // Computing
                case "host"             : return Host;
                case "machinetype"      : return MachineType;
                case "machineimage"     : return MachineImage;
                case "image"            : return MachineImage;

                // Databases
                case "database"         : return Database;
                case "databasecluster"  : return DatabaseCluster;
                case "databaseinstance" : return DatabaseInstance;


                // Networking
                case "network"          : return Network;
                case "ip"               : return NetworkAddress;
                case "networkinterface" : return NetworkInterface;
                case "gateway"          : return NetworkGateway;
                case "subnet"           : return Subnet;

                case "region"           : return Region;
                case "repository"       : return Repository;
                case "role"             : return Role;
                case "user"             : return User;
                case "volume"           : return Volume;
                case "zone"             : return Zone;

                default: throw new Exception("Unexpected resource type: " + text);
            }
        }
    }

    public static class ResourceTypeExtensions
    {
        public static string GetName(this ResourceType type)
        {
            switch (type)
            {
                case App                  : return "app";
                case Bucket               : return "bucket";
                case Database             : return "database";
                case Domain               : return "domain";
                case EncryptionKey        : return "encryptionKey";
                case Function             : return "function";
                case Host                 : return "host";
                case MachineImage         : return "image";
                case MachineType          : return "machineType";
                case Network              : return "network";
                case NetworkInterface     : return "networkInterface";
                case NetworkProxyListener : return "listener";
                case NetworkProxy         : return "proxy";
                case NetworkGateway       : return "gateway";
                case NetworkAddress       : return "ip";
                case Region               : return "region";
                case Repository           : return "repository";
                case Role                 : return "role";
                case Subnet               : return "subnet";
                case User                 : return "user";
                case Volume               : return "volume";
                case Zone                 : return "zone";

                default: throw new Exception("Unexpected type:" + type.ToString());
            }
        }
    }
}
