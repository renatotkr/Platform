using Carbon.Platform.Resources;

namespace Carbon.Platform.Networking
{
    public class RegisterSubnetRequest
    {
        public RegisterSubnetRequest() { }

        public RegisterSubnetRequest(
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