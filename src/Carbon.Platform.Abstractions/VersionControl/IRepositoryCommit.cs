using System;

namespace Carbon.Platform.VersionControl
{
    public interface IRepositoryCommit
    {
        long Id { get; }

        long RepositoryId { get; }

        byte[] Sha1 { get; }

        // byte[] Sha3 { get; set; }

        long AuthorId { get; }

        long CommiterId { get; }
        
        string Message { get; }

        DateTime Created { get; }
    }
}