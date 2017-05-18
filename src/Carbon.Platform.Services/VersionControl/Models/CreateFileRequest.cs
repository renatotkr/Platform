using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform.Storage
{
    public class CreateFileRequest : IRepositoryFile
    {
        [Range(1, 2_199_023_255_552)]
        public long RepositoryId { get; set; }

        [Required]
        public string BranchName { get; set; }

        [Required]
        public string Path { get; set; }

        public long Size { get; set; }

        [Required]
        public byte[] Sha256 { get; set; }

        [Range(1, 2_199_023_255_552)]
        public long CreatorId { get; set; }
    }
}