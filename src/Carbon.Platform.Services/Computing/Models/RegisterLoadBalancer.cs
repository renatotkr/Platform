using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class RegisterLoadBalancer
    {
        public RegisterLoadBalancer(long ownerId, string name, ManagedResource resource)
        {
            Validate.Id(ownerId, nameof(ownerId));
            Validate.NotNullOrEmpty(nameof(name), name);
            
            Name     = name;
            OwnerId  = ownerId;
            Resource = resource;
        }
        
        public long OwnerId { get; }

        public string Name { get; }

        public ManagedResource Resource { get; }
    }
}