using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform.Storage
{
    public class CreateFileRequest : IRepositoryFile
    {
        public long RepositoryId { get; set; }

        [Required]
        public string BranchName { get; set; }

        [Required]
        public string Path { get; set; }

        public long Size { get; set; }

        [Required]
        public byte[] Sha256 { get; set; }

        public long CreatorId { get; set; }
    }
}