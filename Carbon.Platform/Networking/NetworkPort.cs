using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Carbon.Computing
{
    // e.g.
    // 22,80,444
    // 1000-2000
    // 22/tcp,433/tcp

    public class NetworkPortList : Collection<NetworkPort>
    {
        public NetworkPortList(params NetworkPort[] ports)
            : base(ports) { }

        public static NetworkPortList Parse(string text)
        {
            var ports = text.Split(',');

            var list = new NetworkPort[ports.Length];

            for (var i = 0; i < ports.Length; i++)
            {
                list[i] = NetworkPort.Parse(ports[i]);
            }

            return new NetworkPortList(list);
        }

        public override string ToString()
            => string.Join(",", this);
    }

    public struct NetworkPort : IEquatable<NetworkPort>
    {
        public static readonly NetworkPort SSH   = new NetworkPort(22, NetworkProtocal.TCP);
        public static readonly NetworkPort HTTP  = new NetworkPort(80, NetworkProtocal.HTTP);
        public static readonly NetworkPort HTTPS = new NetworkPort(443, NetworkProtocal.HTTPS);

        public NetworkPort(ushort value, NetworkProtocal protocal = NetworkProtocal.TCP)
            : this(value, 0, protocal) { }

        public NetworkPort(ushort start, ushort end, NetworkProtocal protocal = NetworkProtocal.TCP)
        {
            #region Preconditions

            if (start == 0)
                throw new ArgumentException("Must be greater than 0", paramName: nameof(start));

            #endregion

            Start = start;
            End = end;
            Protocal = protocal;
        }

        public ushort Start { get; }

        public ushort End { get; }

        public NetworkProtocal Protocal { get; }

        // 7100/tcp
        // 100-150/tcp
        // 80/http

        public static NetworkPort Parse(string text)
        {
            switch (text)
            {
                case "http"  : return HTTP;
                case "https" : return HTTPS;
            }

            var split = text.Split('/');

            var protocal = NetworkProtocal.Any;

            if (split.Length > 1)
            {
                protocal = ParseProtocal(split[1]);
            }

            if (split[0].Contains("-"))
            {
                var r = split[0].Split('-');

                var start = ushort.Parse(r[0]);
                var end = ushort.Parse(r[1]);

                return new NetworkPort(start, end, protocal);
            }
            else
            {
                var start = ushort.Parse(split[0]);

                return new NetworkPort(start, protocal);
            }
        }

        private static NetworkProtocal ParseProtocal(string text)
        {
            switch (text)
            {
                case "udp"   : return NetworkProtocal.UDP; 
                case "tcp"   : return NetworkProtocal.TCP; 
                case "http"  : return NetworkProtocal.HTTP;
                case "https" : return NetworkProtocal.HTTPS;

                default: throw new Exception("Unexpected protocal:" + text);
            }
        }

        public override string ToString()
        {
            if (Equals(HTTP))  return "http";
            if (Equals(HTTPS)) return "https";

            var sb = new StringBuilder();

            sb.Append(Start);

            if (End != 0)
            {
                sb.Append("-");
                sb.Append(End);
            }

            if (Protocal != NetworkProtocal.Any)
            {
                sb.Append("/");
                sb.Append(Protocal.ToString().ToLower());
            }

            return sb.ToString();
        }

        #region IEquatable<NetworkPort>

        public bool Equals(NetworkPort other)
            => Start == other.Start && End == other.End && Protocal == other.Protocal;

        #endregion

    }
}
