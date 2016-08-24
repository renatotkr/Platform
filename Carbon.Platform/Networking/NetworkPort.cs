using System.Collections.ObjectModel;

namespace Carbon.Networking
{
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
        {
            return string.Join(",", this);
        }
    }

    public struct NetworkPort
    {
        public static readonly NetworkPort SSH = new NetworkPort(22);
        public static readonly NetworkPort HTTPS = new NetworkPort(433);

        public NetworkPort(ushort value, NetworkProtocal protocal = NetworkProtocal.TPC)
            : this(value, 0, protocal) { }

        public NetworkPort(ushort start, ushort end, NetworkProtocal protocal = NetworkProtocal.TPC)
        {      
            Start = start;
            End = end;
            Protocal = protocal;
        }

        public ushort Start { get; }

        public ushort End { get; }

        public NetworkProtocal Protocal { get; }

        // 7100/tcp
        // 100-150/tcp
        public static NetworkPort Parse(string text)
        {
            var split = text.Split('/');

            var protocal = NetworkProtocal.Any;

            if (split.Length > 1)
            {
                switch (split[1])
                {
                    case "UDP"  : protocal = NetworkProtocal.UDP; break;
                    case "TCP"  : protocal = NetworkProtocal.TPC; break;
                    default     : throw new System.Exception("Unexpected protocal:" + split[1]);
                }
            }

            ushort start = 0, end = 0;

            if (split[0].Contains("-"))
            {
                var r = split[0].Split('-');

                start = ushort.Parse(r[0]);
                end = ushort.Parse(r[1]);
            }
            else
            {
                start = ushort.Parse(split[0]);
            }

            return new NetworkPort(start, end, protocal);
        }

        public override string ToString()
        {
            if (End != 0)
            {
                return Start + "-" + End + "/" + Protocal.ToString().ToLower();
            }

            return Start + "/" + Protocal.ToString().ToLower();
        }
    }
}

// e.g.
// 22,80,444
// 1000-2000
// 22/tcp, 433,tcp