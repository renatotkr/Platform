using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class RegisterLoadBalancer
    {
        public RegisterLoadBalancer(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public ManagedResource Resource { get; }
    }
}
