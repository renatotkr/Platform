using System;
using Carbon.Platform.Security;

namespace Carbon.Platform
{
    public class PlatformClient : ApiBase
    {
        public PlatformClient(Uri endpoint, Credential credential)
            : base(endpoint, credential)
        {
            Programs = new ProgramClient(this);
            Hosts    = new HostClient(this);
        }
        
        public ProgramClient Programs { get; }

        public HostClient Hosts { get; }        
    }
}