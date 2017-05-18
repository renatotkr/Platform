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

        [Range(1, 2_199_023_255_552)]
        public long RepositoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public long CreatorId { get; set; }
    }
}