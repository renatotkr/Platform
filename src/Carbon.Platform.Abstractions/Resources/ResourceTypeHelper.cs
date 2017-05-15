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
                case "app"              : return App;

                case "instance"         : return Host;
                case "host"             : return Host;
                case "machinetype"      : return MachineType;
                case "machineimage"     : return MachineImage;

                // Storage
                case "bucket"           : return Bucket;
                case "database"         : return Database;
                case "dek"              : return DataEncryptionKey;
                case "encryptionkey"    : return EncryptionKey;
                case "repository"       : return Repository;

                // Domains & DNS
                case "domain"           : return Domain;
                case "dnszone"          : return DnsZone;
                case "dnsrecord"        : return DnsRecord;

                // Networking
                case "network"          : return Network;
                case "ipAddress"        : return NetworkAddress;
                case "networkinterface" : return NetworkInterface;
                case "subnet"           : return Subnet;

                case "user"             : return User;
                case "volume"           : return Volume;

                default: return (ResourceType)Enum.Parse(typeof(ResourceType), text, ignoreCase: true);
            }
        }
    }
}
