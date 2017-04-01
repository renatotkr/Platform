using System;

namespace Carbon.Platform.Repositories
{
    public interface IRepositoryCommit
    {
        long RepositoryId { get; set; }

        byte[] SHA1 { get; set; }

        byte[] SHA3 { get; set; }

        long AuthorId { get; set; }

        long CommiterId { get; set; }
        
        string Message { get; set; }

        DateTime Created { get; set; }
    }
}