using System;

using Carbon.OAuth2;

namespace Carbon.Platform
{
    public class PlatformClient : ApiBase
    {
        public PlatformClient(Uri endpoint, IAccessToken accessToken)
            : base(endpoint, accessToken)
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