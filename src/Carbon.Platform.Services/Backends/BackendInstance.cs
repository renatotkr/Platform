using System;
using System.Net;

using Carbon.Platform.Computing;

namespace Carbon.Platform
{
    public class BackendInstance
    {
        public BackendInstance(HostInfo host, int port)
        {
            Host    = host ?? throw new ArgumentNullException(nameof(host));
            Address = host.PrivateIp;
            Port    = port;
        }

        public IHost Host { get; }

        public IPAddress Address { get; }

        public int Port { get; }
        
    }
}

// A host may be servicing mutiple backends...
// Shoud we force each app to live in a container?