using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform.Storage
{
    public class DeleteFileRequest : IRepositoryFile
    {
        public DeleteFileRequest() { }

        public DeleteFileRequest(long repositoryId, string branchName, string path)
        {
            RepositoryId = repositoryId;
            BranchName   = branchName;
            Path         = path;
        }

        public long RepositoryId { get; set; }

        [Required]
        public string BranchName { get; set; }

        [Required]
        public string Path { get; set; }

        public long Size => 0;

        public byte[] Sha256 => null;
    }
}