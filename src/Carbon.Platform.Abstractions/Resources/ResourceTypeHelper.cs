using System;

namespace Carbon.Platform.Resources
{
    using static ResourceType;

    public static class ResourceTypeHelper
    {
        public static ResourceType Parse(string text)
        {
            switch (text.ToLower())
            {
                // Computing
                case "instance"         : return Host;
                case "host"             : return Host;
                case "machinetype"      : return MachineType;
                case "machineimage"     : return MachineImage;
                case "program"          : return Program;

                // Storage
                case "bucket"           : return Bucket;
                case "database"         : return Database;
                case "dek"              : return DataEncryptionKey;
                case "encryptionkey"    : return EncryptionKey;
                case "repository"       : return Repository;
                case "volume"           : return Volume;

                // Hosting
                case "domain"           : return Domain;
                case "dnszone"          : return DnsZone;
                case "dnsrecord"        : return DnsRecord;

                // Networking
                case "network"          : return Network;
                case "ipAddress"        : return NetworkAddress;
                case "networkinterface" : return NetworkInterface;
                case "subnet"           : return Subnet;

                // IAM
                case "user"             : return User;

                default: return (ResourceType)Enum.Parse(typeof(ResourceType), text, ignoreCase: true);
            }
        }
    }
}
