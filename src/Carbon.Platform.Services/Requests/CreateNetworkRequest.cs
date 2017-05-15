using System.ComponentModel.DataAnnotations;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Services
{
    public class CreateNetworkRequest
    {
        public CreateNetworkRequest() { }

        public CreateNetworkRequest(
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
    }
}