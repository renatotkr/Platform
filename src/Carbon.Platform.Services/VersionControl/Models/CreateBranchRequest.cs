using System;
using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform.Storage
{
    public class CreateBranchRequest
    {
        public CreateBranchRequest() { }

        public CreateBranchRequest(long repositoryId, string name, long creatorId)
        {
            RepositoryId = repositoryId;
            Name         = name ?? throw new ArgumentNullException(nameof(name));
            CreatorId    = creatorId;
        }

        public long RepositoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public long CreatorId { get; set; }
    }
}