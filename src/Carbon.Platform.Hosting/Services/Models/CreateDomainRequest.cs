using Carbon.Net.Dns;

namespace Carbon.Platform.Hosting
{
    public struct CreateDomainRequest
    {
        public CreateDomainRequest(
            DomainName name, 
            long? ownerId = null, 
            DomainFlags flags = DomainFlags.None)
        {
            Name    = name;
            OwnerId = ownerId;
            Flags   = flags;
        }

        public DomainName Name { get; }
        
        public long? OwnerId { get;  }

        public DomainFlags Flags { get; }
    }
}