using System.ComponentModel.DataAnnotations;

using Carbon.Data.Expressions;

namespace Carbon.Platform.VersionControl
{
    public class CreateBranchRequest
    {
        public long RepositoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public long CreatorId { get; set; }
    }
}