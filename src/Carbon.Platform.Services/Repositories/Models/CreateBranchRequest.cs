using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform.Storage
{
    public class CreateBranchRequest
    {
        public CreateBranchRequest() { }

        public CreateBranchRequest(long repositoryId, string name, long creatorId)
        {
            #region Preconditions

            Validate.Id(repositoryId);

            Validate.NotNullOrEmpty(name, nameof(name));

            Validate.Id(creatorId);

            #endregion

            RepositoryId = repositoryId;
            Name         = name;
            CreatorId    = creatorId;
        }

        [Range(1, 2_199_023_255_552)]
        public long RepositoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public long CreatorId { get; set; }
    }
}