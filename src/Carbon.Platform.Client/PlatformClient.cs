using System;

using Carbon.Security.Tokens;

namespace Carbon.Platform
{
    public class PlatformClient : ApiBase
    {
        public PlatformClient(Uri endpoint, ISecurityToken credential)
            : base(endpoint, credential)
        {
            Programs = new ProgramClient(this);
            Hosts    = new HostClient(this);
        }
        
        public ProgramClient Programs { get; }

        public HostClient Hosts { get; }

        // Volumes

        // Clusters

        // ...
    }
}