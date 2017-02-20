using System;

namespace Carbon.Platform
{
    public enum ResourceType
    {
        App              = 1,
        Bucket           = 2,
        Database         = 3, // db?
        Domain           = 4,
        Function         = 5,
        Image            = 6,
        Instance         = 7,  // Host Instance          
        // Logs          = 8, 
        Network          = 20, // AKA VPC
        NetworkInterface = 21, // network-interface
        Subnet           = 22, // AKA subnetwork
        User             = 30,
        Volume           = 40,
        Region           = 41,
        Zone             = 42, // AKA AvailabilityZone -- amnz:zone:us-east-1b

        MachineType      = 50, // machinetype
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
