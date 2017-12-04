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
            #region Preconditions

            if (port == 0)
                throw new ArgumentException("May not be 0", nameof(port));

            #endregion

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
        public ApplicationProtocal Scheme { get; }

        [DataMember(Name = "host", Order = 2)]
        public string Host { get; }

        [DataMember(Name = "port", Order = 3)]
        public ushort Port { get; }

        public static Listener Parse(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            #endregion

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

            throw new Exception("Unsupported format:" + text);
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

        public bool Equals(Listener other)
        {
            return Host == other.Host && Port == other.Port && Scheme == other.Scheme;
        }

        #endregion
    }
}
