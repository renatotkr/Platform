using System;

namespace Carbon.Platform
{
    using static ResourceType;

    public static class ResourceTypeHelper
    {
        public static ResourceType Parse(string text)
        {
            switch (text.ToLower())
            {
                case "app"              : return App;
                case "function"         : return Function;

                // Computing
                case "host"             : return Host;
                case "machinetype"      : return MachineType;
                case "machineimage"     : return MachineImage;

                // Storage
                case "blob"             : return Blob;
                case "bucket"           : return Bucket;
                case "database"         : return Database;
                case "encryptionkey"    : return EncryptionKey;
                case "repository"       : return Repository;

                // Domains & DNS
                case "domain"           : return Domain;
                case "dnszone"          : return DnsZone;
                case "dnsrecord"        : return DnsRecord;

                // Networking
                case "network"          : return Network;
                case "ip"               : return NetworkAddress;
                case "networkinterface" : return NetworkInterface;
                case "gateway"          : return NetworkGateway;
                case "subnet"           : return Subnet;

                case "region"           : return Region;
                // case "role"             : return Role;
                case "user"             : return User;
                case "volume"           : return Volume;
                case "zone"             : return Zone;

                default: throw new Exception("Unexpected resource type: " + text);
            }
        }
    }
}
