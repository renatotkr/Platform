using System;

namespace Carbon.Platform.Resources
{
    using static ResourceType;

    public static class ResourceTypeExtensions
    {
        public static string GetName(this ResourceType type)
        {
            switch (type)
            {
                case EncryptionKey        : return "encryptionKey";
                case MachineImage         : return "machineImage";
                case MachineType          : return "machineType";
                case NetworkInterface     : return "networkInterface";
                case LoadBalancerListener : return "listener";
                case NetworkGateway       : return "gateway";
                case NetworkAddress       : return "ip";
                

                default: return type.ToString().ToLower();
            }
        }
    }
}
