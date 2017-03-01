using System;

namespace Carbon.Platform
{
    // Can we limit to 63?

    [Flags]
    public enum ResourceType : long
    {
        App              = 1L << 1,
        Bucket           = 1L << 2,
        Database         = 1L << 3, // db?
        Domain           = 1L << 4,
        DnsZone          = 1L << 5,
        Function         = 1L << 6,
        Image            = 1L << 7,
        Instance         = 1L << 8,  // host-instance          
        // Logs          = 1L << 9, 
        MachineType      = 1L << 14, // machine-type
        Network          = 1L << 20, // AKA VPC
        NetworkGateway   = 1L << 21, // gateway
        NetworkInterface = 1L << 22, // network-interface
        Subnet           = 1L << 23, // AKA subnetwork
        User             = 1L << 30,
        Volume           = 1L << 40,
        Region           = 1L << 41,
        Zone             = 1L << 42, // AKA AvailabilityZone -- amnz:zone:us-east-1b
    }

    // Stream (Kineses)
    // EncrpytionKey (KMS)

    // ContainerInstance,
    
    public static class ResourceTypeHelper
    {
        public static ResourceType Parse(string text)
        {
            switch(text)
            {
                case "app"              : return ResourceType.App;
                case "bucket"           : return ResourceType.Bucket;
                case "database"         : return ResourceType.Database;
                case "function"         : return ResourceType.Function;
                case "image"            : return ResourceType.Image;
                case "instance"         : return ResourceType.Instance;
                case "network"          : return ResourceType.Network;
                case "networkinterface" : return ResourceType.NetworkInterface;
                case "region"           : return ResourceType.Region;
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
                case ResourceType.Function         : return "function";
                case ResourceType.Instance         : return "instance";
                case ResourceType.Image            : return "image";
                case ResourceType.Network          : return "network";
                case ResourceType.NetworkInterface : return "networkinterface";
                case ResourceType.Region           : return "region";
                case ResourceType.Subnet           : return "subnet";
                case ResourceType.User             : return "user";
                case ResourceType.Volume           : return "volume";
                case ResourceType.Zone             : return "zone";

                default: throw new Exception("Unexpected type:" + type.ToString());
            }
        }
    }
}
