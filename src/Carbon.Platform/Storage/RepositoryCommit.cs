using System;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    [Dataset("RepositoryCommits", Schema = "Storage")]
    [UniqueIndex("repositoryId", "sha1")]
    public class RepositoryCommit : IRepositoryCommit
    {
        public RepositoryCommit() { }

        public RepositoryCommit(
            long id, 
            long repositoryId,
            byte[] sha1, 
            string message = null,
            long? authorId = null,
            long? commiterId = null)
        {
            Id           = id;
            RepositoryId = repositoryId;
            Sha1         = sha1 ?? throw new ArgumentNullException(nameof(sha1));
            Message      = message;
            AuthorId     = authorId ?? 0;
            CommiterId   = commiterId;
        }

        // repositoryId + sequence
        [Member("id"), Key]
        public long Id { get; }

        [Member("repositoryId")]
        public long RepositoryId { get; }

        [Member("sha1"), FixedSize(20)]
        public byte[] Sha1 { get; }

        [Member("message")]
        public string Message { get; }

        [Member("authorId")]
        public long AuthorId { get; }

        [Member("committerId")]
        public long? CommiterId { get; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.RepositoryCommit;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        // Commits are immutable
        // No need for modified or delete

        #endregion
    }
}

// Authored?
// Commited?

// Parents
// Tree

// TODO: Migrate to SHA3 as the secondary key once GIT stablizes

// Note: Linux has the most known number of commits: 387,992 (below the scoped sequence number limit: 4M)