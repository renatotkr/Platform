
using Carbon.Platform.Resources;

namespace Carbon.CI
{
    public class CreateProjectRequest
    {
        public CreateProjectRequest() { }

        public CreateProjectRequest(long ownerId, string name, long repositoryId)
        {
            Validate.Id(ownerId, nameof(ownerId));
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.Id(repositoryId, nameof(repositoryId));

            Name         = name;
            OwnerId      = ownerId;
            RepositoryId = repositoryId;
        }

        public long OwnerId { get; set; }

        public string Name { get; set; }

        public long RepositoryId { get; set; }

        public ManagedResource Resource { get; set; }
    }
}