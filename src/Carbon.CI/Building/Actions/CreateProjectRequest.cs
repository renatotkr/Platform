using System;
using Carbon.Platform.Resources;

namespace Carbon.CI
{
    public class CreateProjectRequest
    {
        public CreateProjectRequest() { }

        public CreateProjectRequest(string name, long ownerId, long repositoryId)
        {
            Name         = name ?? throw new ArgumentNullException(nameof(name));
            OwnerId      = ownerId;
            RepositoryId = repositoryId;
        }

        public long OwnerId { get; set; }

        public string Name { get; set; }

        public long RepositoryId { get; set; }

        public ManagedResource Resource { get; set; }
    }
}