using System;

namespace Carbon.Platform.Hosting
{
    public class CreateDomainRequest
    {
        public CreateDomainRequest() { }

        public CreateDomainRequest(
            string name, 
            long? ownerId = null)
        {
            Name    = name ?? throw new ArgumentNullException(nameof(name));
            OwnerId = ownerId;
        }

        public string Name { get; set; }
        
        public long? OwnerId { get; set; }
    }
}