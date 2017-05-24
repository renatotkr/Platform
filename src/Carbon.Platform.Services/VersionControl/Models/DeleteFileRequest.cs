using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform.Storage
{
    public class DeleteFileRequest : IRepositoryFile
    {
        public DeleteFileRequest() { }

        public DeleteFileRequest(long branchId, string path)
        {
            #region Preconditions

            Validate.Id(branchId, nameof(branchId));

            Validate.NotNullOrEmpty(path, nameof(path));

            #endregion

            BranchId = branchId;
            Path     = path;
        }

        [Required]
        public long BranchId { get; set; }
       
        [Required]
        [StringLength(255)]
        public string Path { get; set; }

        public long Size => 0;

        public byte[] Sha256 => null;
    }
}