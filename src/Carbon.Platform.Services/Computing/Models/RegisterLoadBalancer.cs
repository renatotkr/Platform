using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public readonly struct RegisterLoadBalancer
    {
        public RegisterLoadBalancer(string name, long ownerId, ManagedResource resource)
        {
            Validate.NotNullOrEmpty(nameof(name), name);
            Validate.Id(ownerId, nameof(ownerId));

            Name     = name;
            OwnerId  = ownerId;
            Resource = resource;
        }

        public string Name { get; }

        public long OwnerId { get; }

        public ManagedResource Resource { get; }
    }
}