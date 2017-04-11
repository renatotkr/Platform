using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.VersionControl
{
    [Dataset("RepositoryCommits")]
    public class RepositoryCommit : ICommit
    {
        public RepositoryCommit() { }

        public RepositoryCommit(long id, byte[] sha1, string message = null)
        {
            Id   = id;
            Sha1 = sha1 ?? throw new ArgumentNullException(nameof(sha1));
            Message = message;
        }

        // repositoryId + sequence
        [Member("id"), Key]
        public long Id { get; }

        [Member("authorId")]
        public long AuthorId { get; set; }

        [Member("committerId")]
        public long CommiterId { get; set; }

        [Member("message")]
        public string Message { get; }

        public long RepositoryId => ScopedId.GetScope(Id);

        #region Hashes

        [Member("sha1", TypeName = "binary(20)")]
        [Indexed]
        public byte[] Sha1 { get; }

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
// CommitDate?


// Parents
// Tree


// TODO: Migrate to SHA3 as the secondary key once GIT stablizes

// Note: Linux has the most known number of commits: 387,992
// This is well below 4M max for the sequence