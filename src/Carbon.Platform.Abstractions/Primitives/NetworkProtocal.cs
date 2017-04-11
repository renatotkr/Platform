using System;

namespace Carbon.Net
{
    // https://www.iana.org/assignments/protocol-numbers/protocol-numbers.xml

    public enum NetworkProtocal : byte
    {
        ICMP = 1,
        TCP  = 6,
        UDP  = 17
    }

    public static class NetworkProtocalHelper
    {
        private static NetworkProtocal Parse(string text)
        {
            switch (text)
            {
                case "icmp" : return NetworkProtocal.ICMP;
                case "tcp"  : return NetworkProtocal.TCP;
                case "udp"  : return NetworkProtocal.UDP;

                default: throw new Exception("Unexpected protocal:" + text);
            }
        }
    }
}