using System;
using System.Runtime.Serialization;
using System.Text;

namespace Carbon.Net
{
    // http://localhost:60000
    // https://*:5004

    [DataContract]
    public readonly struct Listener : IEquatable<Listener>
    {
        public static readonly Listener SSH   = new Listener(ApplicationProtocal.SSH,   22);
        public static readonly Listener HTTP  = new Listener(ApplicationProtocal.HTTP,  80);
        public static readonly Listener HTTPS = new Listener(ApplicationProtocal.HTTPS, 433);
        
        // Certificate?

        public Listener(ApplicationProtocal protocal, ushort port)
        {
            if (port == 0)
            {
                throw new ArgumentException("May not be 0", nameof(port));
            }

            Scheme = protocal;
            Port   = port;
            Host   = "*";
        }

        public Listener(ApplicationProtocal protocal, string host, ushort port)
        {
            Scheme = protocal;
            Host   = host;
            Port   = port;
        }

        [DataMember(Name = "scheme", Order = 1)]
        public readonly ApplicationProtocal Scheme;

        [DataMember(Name = "host", Order = 2)]
        public readonly string Host;

        [DataMember(Name = "port", Order = 3)]
        public readonly ushort Port;

        public static Listener Parse(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text.Contains("://"))
            {
                var parts = text.Split(new[] { "://" }, StringSplitOptions.None);

                var hostParts = parts[1].Split(':');
                var protocal = ApplicationProtocalHelper.Parse(parts[0]);
                
                return new Listener(
                    protocal : protocal,
                    host     : hostParts[0],
                    port     : hostParts.Length == 2 ? ushort.Parse(hostParts[1]) : (ushort)protocal
                );
            }

            throw new Exception("Invalid format:" + text);
        }
        
        public override string ToString()
        {
            if (Equals(HTTP))  return "http";
            if (Equals(HTTPS)) return "https";

            if (Host != null)
            {
                return Scheme.ToString().ToLower() + "://" + Host + ":" + Port;
            }

            var sb = new StringBuilder();

            sb.Append(Port);

            if (Scheme != default)
            {
                sb.Append('/');
                sb.Append(Scheme.Canonicalize());
            }

            return sb.ToString();
        }

        #region IEquatable<NetworkPort>

        public bool Equals(Listener other) =>
            Host == other.Host && 
            Port == other.Port && 
            Scheme == other.Scheme;

        #endregion
    }
}
