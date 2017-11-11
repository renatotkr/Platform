using System.ComponentModel.DataAnnotations;

namespace Carbon.CI
{
    public class CreateBranchRequest
    {
        public CreateBranchRequest() { }

        public CreateBranchRequest(long repositoryId, string name)
        {
            Validate.Id(repositoryId, nameof(repositoryId));
            Validate.NotNullOrEmpty(name, nameof(name));

            RepositoryId = repositoryId;
            Name         = name;
        }

        public long RepositoryId { get; set; }

        [Required]
        public string Name { get; set;  }
    }
}

// NOTE: keep set properties for JSON binding