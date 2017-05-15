using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform.VersionControl
{
    public class DeleteFileRequest : IRepositoryFile
    {
        public long RepositoryId { get; set; }

        [Required]
        public string BranchName { get; set; }

        [Required]
        public string Path { get; set; }

        public long Size => 0;

        public byte[] Sha256 => null;
    }
}