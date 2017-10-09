using System;

namespace Carbon.Platform.Hosting
{
    public class CreateDomainRequest
    {
        public CreateDomainRequest(string name, long? ownerId = null, DomainFlags flags = DomainFlags.None)
        {
            Name    = name ?? throw new ArgumentNullException(nameof(name));
            OwnerId = ownerId;
            Flags   = flags;
        }

        public string Name { get; }
        
        public long? OwnerId { get;  }

        public DomainFlags Flags { get; }
    }
}