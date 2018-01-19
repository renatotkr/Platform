using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class RegisterLoadBalancer
    {
        public RegisterLoadBalancer(long ownerId, string name, ManagedResource resource)
        {
            Ensure.IsValidId(ownerId, nameof(ownerId));
            Ensure.NotNullOrEmpty(nameof(name), name);
            
            Name     = name;
            OwnerId  = ownerId;
            Resource = resource;
        }
        
        public long OwnerId { get; }

        public string Name { get; }

        public ManagedResource Resource { get; }
    }
}