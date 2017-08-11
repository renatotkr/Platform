using System;

using Carbon.Platform.Resources;

namespace Carbon.CI
{
    public class CreateProjectRequest
    {
        public CreateProjectRequest() { }

        public CreateProjectRequest(string name, long ownerId, long repositoryId)
        {
            #region Preconditions

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            #endregion

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