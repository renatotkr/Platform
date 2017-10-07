using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class RegisterLoadBalancer
    {
        public RegisterLoadBalancer(string name, ManagedResource resource)
        {
            Name     = name;
            Resource = resource;
        }

        public string Name { get; }

        public ManagedResource Resource { get; }
    }
}