using System;

namespace Carbon.Platform
{
    using static ResourceType;

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
                case MachineImage         : return "machineimage";
                case MachineType          : return "machineType";
                case Network              : return "network";
                case NetworkInterface     : return "networkInterface";
                case NetworkProxyListener : return "listener";
                case LoadBalancer         : return "proxy";
                case NetworkGateway       : return "gateway";
                case NetworkAddress       : return "ip";
                case Queue                : return "queue";
                case Region               : return "region";
                case Repository           : return "repository";
                case Subnet               : return "subnet";
                case User                 : return "user";
                case Volume               : return "volume";
                case Zone                 : return "zone";

                default: return type.ToString().ToLower();
            }
        }
    }
}
