using System;

namespace Carbon.Hosting.IIS
{
    using Platform.Networking;

    // TODO: Replace with listener?

    public class WebBinding
    {
        public WebBinding(Listener listener)
        {
            Port = listener.Port;
            HostName = listener.Host;
        }

        public WebBinding(int port, string hostName = null, string ip = null)
        {
            #region Preconditions

            if (port < 80) throw new ArgumentException(paramName: nameof(port), message: "Must be 80 or greater");

            #endregion

            HostName = hostName;
            Port = port;
            Ip = ip;
        }

        public string Protocol => "http";

        public string HostName { get; }

        public int Port { get; }

        public string Ip { get; }

        public static WebBinding Parse(string text)
        {
            #region Preconditions

            if (text == null) throw new ArgumentNullException(nameof(text));

            #endregion

            var parts = text.Split(':');

            if (parts.Length < 3) throw new Exception("must be at least 3 parts");

            var ip = parts[0];
            var port = int.Parse(parts[1]);
            var host = parts[2];

            return new WebBinding(port, host, ip);
        }

        // 192.169.4.2:80:hostName

        public string GetInformation()
            => string.Format("{0}:{1}:{2}", 
                /*0*/ Ip ?? "*",
                /*1*/ Port,
                /*2*/ HostName ?? "");

        public override string ToString() 
            => GetInformation();
    }
}