using System;

namespace Carbon.Net
{
    [Flags]
    public enum ProxyRoles
    {
        None           = 0,
        LoadBalancer   = 1 << 0,
        Firewall       = 1 << 1, // Enforces Network Rules
        IP6Termination = 1 << 2, // Terminates Ip6 traffic
        SslTermination = 1 << 3  // Terminates Ssl traffic
    }
}