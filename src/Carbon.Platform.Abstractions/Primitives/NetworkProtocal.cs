using System;

namespace Carbon.Net
{
    using static NetworkProtocal;

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
                case "icmp" : return ICMP;
                case "tcp"  : return TCP;
                case "udp"  : return UDP;

                default: throw new Exception("Unexpected protocal:" + text);
            }
        }
    }
}