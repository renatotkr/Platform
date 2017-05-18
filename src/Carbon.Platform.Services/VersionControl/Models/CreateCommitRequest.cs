using System;
using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform.VersionControl
{
    public class CreateCommitRequest
    {
        public long RepositoryId { get; set; }

        [Required]
        public byte[] Sha1 { get; set; }

        public string Message { get; set; }

        public DateTime Created { get; set; }
    }
}