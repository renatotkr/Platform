using System;
using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform.Storage
{
    public class CreateCommitRequest
    {
        public CreateCommitRequest() { }

        public CreateCommitRequest(long repositoryId, byte[] sha1, string message, DateTime created)
        {
            #region Preconditions

            Validate.Id(repositoryId, nameof(repositoryId));

            Validate.NotNull(sha1, nameof(sha1));

            #endregion

            RepositoryId = repositoryId;
            Sha1         = sha1;
            Message      = message;
            Created      = created;
        }

        [Range(1, 2_199_023_255_552)]
        public long RepositoryId { get; set; }

        [Required]
        public byte[] Sha1 { get; set; }

        public string Message { get; set; }

        public DateTime Created { get; set; }
    }
}