using Carbon.Platform.Resources;

namespace Carbon.Platform.Services
{
    public class CreateSubnetRequest
    {
        public CreateSubnetRequest() { }

        public CreateSubnetRequest(
            string cidr,
            long networkId,
            ManagedResource resource)
        {

            Cidr = cidr;
            NetworkId = networkId;
            Resource = resource;
        }

        public string Cidr { get; set; }

        public long NetworkId { get; set; }

        public ManagedResource Resource { get; set; }
    }
}