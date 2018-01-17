using System;

namespace Carbon.CI
{
    public class CreateCommitRequest
    {
        public CreateCommitRequest(
            long repositoryId, 
            byte[] sha1, 
            string message,
            long authorId, 
            long commiterId,
            DateTime created)
        {
            Ensure.IsValidId(repositoryId, nameof(repositoryId));

            RepositoryId = repositoryId;
            Sha1         = sha1 ?? throw new ArgumentNullException(nameof(sha1));
            Message      = message;
            AuthorId     = authorId;
            CommiterId   = commiterId;
            Created      = created;
        }

        public long RepositoryId { get; }

        public byte[] Sha1 { get; }

        public long AuthorId { get; }

        public long CommiterId { get; }

        public string Message { get; }

        public DateTime Created { get; }
    }
}