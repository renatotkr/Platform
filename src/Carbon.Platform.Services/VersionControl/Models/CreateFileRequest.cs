using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform.Storage
{
    public class CreateFileRequest : IRepositoryFile
    {
        public CreateFileRequest() { }

        public CreateFileRequest(
            long branchId,
            string path,
            long size,
            byte[] sha256,
            long creatorId)
        {
            #region Preconditions

            Validate.NotNullOrEmpty(path, nameof(path));

            Validate.Id(creatorId, nameof(creatorId));

            #endregion

            BranchId  = branchId;
            Path      = path;
            Size      = size;
            Sha256    = sha256;
            CreatorId = creatorId;
        }

        [Required]
        public long BranchId { get; set; }

        [Required]
        public string Path { get; set; }

        public long Size { get; set; }

        [Required]
        public byte[] Sha256 { get; set; }

        [Range(1, 2_199_023_255_552)]
        public long CreatorId { get; set; }
    }
}