using System;

using Carbon.Data.Annotations;
using Carbon.Platform.Sequences;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    [Dataset("RepositoryCommits")]
    public class RepositoryCommit : IRepositoryCommit
    {
        public RepositoryCommit() { }

        public RepositoryCommit(
            long id, 
            byte[] sha1, 
            string message = null,
            long? authorId = null,
            long? commiterId = null)
        {
            Id         = id;
            Sha1       = sha1 ?? throw new ArgumentNullException(nameof(sha1));
            Message    = message;
            AuthorId   = authorId ?? 0;
            CommiterId = commiterId;
        }

        // repositoryId + sequence
        [Member("id"), Key]
        public long Id { get; }

        [Member("authorId")]
        public long AuthorId { get; }

        [Member("committerId")]
        public long? CommiterId { get; }

        [Member("message")]
        public string Message { get; }

        [Member("sha1", TypeName = "binary(20)")]
        [Indexed]
        public byte[] Sha1 { get; }

        // future proof when GIT moves to sha3
        [Member("sha3", TypeName = "binary(32)")]
        [Indexed]
        public byte[] Sha3 { get; }

        public long RepositoryId => ScopedId.GetScope(Id);
        
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