using Carbon.Platform.Resources;

namespace Carbon.Platform.VersionControl
{
    public class CreateRepositoryRequest
    {
        public CreateRepositoryRequest() { }

        public CreateRepositoryRequest(
            string name, 
            long ownerId,
            ManagedResource resource)
        {
            Name     = name;
            OwnerId  = ownerId;
            Resource = resource;
        }

        public string Name { get; set; }

        public long OwnerId { get; set; }

        public ManagedResource Resource { get; set; }
    }
}