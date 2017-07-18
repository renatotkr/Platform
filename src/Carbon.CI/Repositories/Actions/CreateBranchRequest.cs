using System.ComponentModel.DataAnnotations;

namespace Carbon.CI
{
    public class CreateBranchRequest
    {
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

        public long RepositoryId { get; }

        [Required]
        public string Name { get; }

        public long CreatorId { get; }
    }
}