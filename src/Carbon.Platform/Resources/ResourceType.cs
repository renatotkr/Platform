using System;

namespace Carbon.Platform
{
    [Flags]
    public enum ResourceType : long
    {
        App              = 1L << 1,
        Bucket           = 1L << 2,
        Database         = 1L << 3,
        Domain           = 1L << 4,
        DnsZone          = 1L << 5,
        EncryptionKey    = 1L << 6,
        Function         = 1L << 7,
        Host             = 1L << 8,  // includes metal, vm instances, and container instances
        Image            = 1L << 9,
        MachineType      = 1L << 15, 
        Network          = 1L << 20, // AKA VPC
        NetworkAddress   = 1L << 21, // AKA IPAddress
        NetworkGateway   = 1L << 22, 
        NetworkInterface = 1L << 23,
        NetworkListener  = 1L << 24,
        NetworkProxy     = 1L << 25,
        NetworkRule      = 1L << 26, // Enforced by network or proxy...
        Region           = 1L << 30,
        SslCertificate   = 1L << 31,
        Subnet           = 1L << 32, // AKA subnetwork
        Role             = 1L << 40,
        User             = 1L << 41,
        Volume           = 1L << 50,
        Zone             = 1L << 60, // AKA AvailabilityZone -- amnz:zone:us-east-1b
    }

    // Logs (?)
    // Stream (Kineses)

    public static class ResourceTypeHelper
    {
        public static ResourceType Parse(string text)
        {
            switch (text.ToLower())
            {
                case "app"              : return ResourceType.App;
                case "bucket"           : return ResourceType.Bucket;
                case "database"         : return ResourceType.Database;
                case "domain"           : return ResourceType.Domain;
                case "encryptionkey"    : return ResourceType.EncryptionKey;
                case "function"         : return ResourceType.Function;
                case "image"            : return ResourceType.Image;
                case "host"             : return ResourceType.Host;
                case "machineType"      : return ResourceType.MachineType;
                case "network"          : return ResourceType.Network;
                case "networkinterface" : return ResourceType.NetworkInterface;
                case "gateway"          : return ResourceType.NetworkGateway;
                case "region"           : return ResourceType.Region;
                case "role"             : return ResourceType.Role;
                case "subnet"           : return ResourceType.Subnet;
                case "user"             : return ResourceType.User;
                case "volume"           : return ResourceType.Volume;
                case "zone"             : return ResourceType.Zone;
                case "machinetype"      : return ResourceType.MachineType;

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
                case ResourceType.App              : return "app";
                case ResourceType.Bucket           : return "bucket";
                case ResourceType.Database         : return "database";
                case ResourceType.Domain           : return "domain";
                case ResourceType.EncryptionKey    : return "encryptionKey";
                case ResourceType.Function         : return "function";
                case ResourceType.Host             : return "host";
                case ResourceType.Image            : return "image";
                case ResourceType.MachineType      : return "machineType";
                case ResourceType.Network          : return "network";
                case ResourceType.NetworkInterface : return "networkInterface";
                case ResourceType.NetworkListener  : return "listener";
                case ResourceType.NetworkProxy     : return "proxy";
                case ResourceType.NetworkGateway   : return "gateway";
                case ResourceType.NetworkAddress   : return "ip";
                case ResourceType.Region           : return "region";
                case ResourceType.Role             : return "role";
                case ResourceType.Subnet           : return "subnet";
                case ResourceType.User             : return "user";
                case ResourceType.Volume           : return "volume";
                case ResourceType.Zone             : return "zone";

                default: throw new Exception("Unexpected type:" + type.ToString());
            }
        }
    }
}
