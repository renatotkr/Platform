using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform.Hosting
{
    public class CreateDomainRequest
    {
        public CreateDomainRequest() { }

        public CreateDomainRequest(
            string name, 
            long? ownerId = null,
            long? environmentId = null,
            long? originId = null,
            DomainFlags flags = default)
        {
            Validate.NotNullOrEmpty(name, nameof(name));

            // Domains may be added without owners...

            Name          = name;
            OwnerId       = ownerId;
            EnvironmentId = environmentId;
            OriginId      = originId;
            Flags         = flags;
        }
        
        [Required, StringLength(253)]
        public string Name { get; set; }
        
        public long? OwnerId { get; set; }
        
        public long? EnvironmentId { get; set; } 
        
        public long? OriginId { get; set; }

        public DomainFlags Flags { get; set; }
    }
}