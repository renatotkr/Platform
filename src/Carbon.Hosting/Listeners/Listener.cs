using System;
using System.Text;

namespace Carbon.Net
{
    // http://localhost:60000
    // https://*:5004
    // {ip}:{port}:{hostName}

    public struct Listener : IEquatable<Listener>
    {
        public static readonly Listener SSH   = new Listener(22,  ApplicationProtocal.SSH);
        public static readonly Listener HTTP  = new Listener(80,  ApplicationProtocal.HTTP);
        public static readonly Listener HTTPS = new Listener(443, ApplicationProtocal.HTTPS);
        
        // Certificate?

        public Listener(ushort port, ApplicationProtocal protocal)
        {
            #region Preconditions

            if (port == 0)
                throw new ArgumentException("May not be 0", nameof(port));

            #endregion

            Protocal = protocal;
            Port     = port;
            Host     = null;
            Ip       = null;
        }

        public Listener(ApplicationProtocal protocal, string host, ushort port)
        {
            Protocal = protocal;
            Host     = host;
            Ip       = null;
            Port     = port;
        }

        public ApplicationProtocal Protocal { get; }

        public ushort Port { get; }

        public string Host { get; }

        // * | 192.168.1.1
        public string Ip { get; }

        // 7100/tcp
        // 100-150/tcp
        // 80/http

        public static Listener Parse(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            #endregion

            if (text.StartsWith("http://"))
            {
                var url = new Uri(text);

                return new Listener(ApplicationProtocal.HTTP, url.Host, (ushort)url.Port);
            }

            switch (text)
            {
                case "http"  : return HTTP;
                case "https" : return HTTPS;
            }

            var split = text.Split('/'); // '/'

            ApplicationProtocal protocal = default(ApplicationProtocal);

            if (split.Length > 1)
            {
                protocal = ApplicationProtocalHelper.Parse(split[1]);
            }
            
            return new Listener(ushort.Parse(split[0]), protocal);
        }
        
        public override string ToString()
        {
            if (Equals(HTTP))  return "http";
            if (Equals(HTTPS)) return "https";

            if (Host != null)
            {
                return Protocal.ToString().ToLower() + "://" + Host + ":" + Port;
            }

            var sb = new StringBuilder();

            sb.Append(Port);

            if (Protocal != default(ApplicationProtocal))
            {
                sb.Append("/");
                sb.Append(Protocal.Canonicalize());
            }

            return sb.ToString();
        }

        #region IEquatable<NetworkPort>

        public bool Equals(Listener other)
        {
            return Host == other.Host && Port == other.Port && Protocal == other.Protocal;
        }

        #endregion
    }
}
