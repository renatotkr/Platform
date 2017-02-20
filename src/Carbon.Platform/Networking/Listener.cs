using System;
using System.Text;

namespace Carbon.Platform.Networking
{
    using Extensions;

    // 22/tcp
    // 433/tcp
    // http://localhost:60000
    // https://*:5004

    public struct Listener : IEquatable<Listener>
    {
        public static readonly Listener SSH   = new Listener(22, NetworkProtocal.TCP);
        public static readonly Listener HTTP  = new Listener(80, NetworkProtocal.HTTP);
        public static readonly Listener HTTPS = new Listener(443, NetworkProtocal.HTTPS);
        
        // Certificate?

        public Listener(int port, NetworkProtocal protocal = NetworkProtocal.TCP)
        {
            #region Preconditions

            if (port <= 0)
                throw new ArgumentException("Must be greater than 0", nameof(port));

            #endregion

            Port = port;
            Host = null;
            Protocal = protocal;
        }

        public Listener(NetworkProtocal protocal, string host, int port)
        {
            Protocal = protocal;
            Host = host;
            Port = port;
        }

        public int Port { get; }

        public string Host { get; }

        public NetworkProtocal Protocal { get; }

        // 7100/tcp
        // 100-150/tcp
        // 80/http

        public static Listener Parse(string text)
        {
            if (text.StartsWith("http://"))
            {
                var url = new Uri(text);

                return new Listener(NetworkProtocal.HTTP, url.Host, url.Port);
            }

            switch (text)
            {
                case "http"  : return HTTP;
                case "https" : return HTTPS;
            }

            var split = text.Split(Seperator.ForwardSlash); // '/'

            var protocal = NetworkProtocal.Any;

            if (split.Length > 1)
            {
                protocal = ParseProtocal(split[1]);
            }

            if (split[0].Contains("-"))
            {
                throw new Exception("Port ranges are not supported");
                /*
                var r = split[0].Split(Seperator.Dash);

                var start = ushort.Parse(r[0]);
                var end = ushort.Parse(r[1]);

                return new Listener(start, end, protocal);
                */
            }
            else
            {
                return new Listener(int.Parse(split[0]), protocal);
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

            if (Host != null) return "http://" + Host + ":" + Port;

            var sb = new StringBuilder();

            sb.Append(Port);

            /*
            if (End != 0)
            {
                sb.Append("-");
                sb.Append(End);
            }
            */

            if (Protocal != NetworkProtocal.Any)
            {
                sb.Append("/");
                sb.Append(Protocal.ToString().ToLower());
            }

            return sb.ToString();
        }

        #region IEquatable<NetworkPort>

        public bool Equals(Listener other)
            => Host == other.Host && Port == other.Port && Protocal == other.Protocal;

        #endregion
    }
}
