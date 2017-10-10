using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public struct RegisterLoadBalancer
    {
        public RegisterLoadBalancer(string name, long ownerId, ManagedResource resource)
        {
            Name     = name;
            OwnerId  = ownerId;
            Resource = resource;
        }

        public string Name { get; }

        public long OwnerId { get; }

        public ManagedResource Resource { get; }
    }
}