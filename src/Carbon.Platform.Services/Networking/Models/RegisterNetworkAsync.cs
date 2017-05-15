using System.ComponentModel.DataAnnotations;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public class RegisterNetworkAsync
    {
        public RegisterNetworkAsync() { }

        public RegisterNetworkAsync(
            string cidr,
            ManagedResource resource)
        {
           
            Cidr     = cidr;
            Resource = resource;
        }

        [Required]
        public string Cidr { get; set; }

        [Required]
        public ManagedResource Resource { get; set; }

        // TODO
        public long OwnerId { get; set; }
    }
}