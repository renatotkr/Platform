using System;
using System.ComponentModel.DataAnnotations;

namespace Carbon.CI
{
    public class CreateCommitRequest
    {
        public CreateCommitRequest(long repositoryId, byte[] sha1, string message, long authorId, DateTime created)
        {
            #region Preconditions

            if (repositoryId <= 0)
                throw new ArgumentException("Must be > 0", nameof(repositoryId));

            #endregion

            RepositoryId = repositoryId;
            Sha1         = sha1 ?? throw new ArgumentNullException(nameof(sha1));
            Message      = message;
            AuthorId     = authorId;
            Created      = created;
        }

        public long RepositoryId { get; }

        public byte[] Sha1 { get; }

        public long AuthorId { get; }

        public string Message { get; }

        public DateTime Created { get; }
    }
}