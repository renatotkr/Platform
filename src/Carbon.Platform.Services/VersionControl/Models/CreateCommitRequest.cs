using System.ComponentModel.DataAnnotations;

using Carbon.Data.Expressions;

namespace Carbon.Platform.VersionControl
{
    public class CreateCommitRequest
    {
        public long RepositoryId { get; set; }

        [Required]
        public byte[] Sha1 { get; set; }

        public string Message { get; set; }
    }
}