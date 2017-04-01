using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Repositories
{
    [Dataset("RepositoryCommits")]
    public class RepositoryCommit : IRepositoryCommit
    {
        [Member("repositoryId"), Key]
        public long RepositoryId { get; set; }

        [Member("sha1", TypeName = "binary(20)"), Key]
        public byte[] SHA1 { get; set; }

        [Member("authorId")]
        public long AuthorId { get; set; }

        [Member("committerId")]
        public long CommiterId { get; set; }

        [Member("commited")] // Commit date?
        public DateTime Commited { get; set; }
        
        [Member("message")]
        public string Message { get; set; }
        
        [Member("sha3", TypeName = "binary(32)"), Optional]
        public byte[] SHA3 { get; set; }

        // Parents
        // Tree

        [Member("created")]
        public DateTime Created { get; set; }
    }
}

// TODO: Migrate to SHA3 as the secondary key once GIT stablizes
