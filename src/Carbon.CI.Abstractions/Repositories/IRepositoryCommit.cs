using System;

namespace Carbon.CI
{
    public interface IRepositoryCommit 
    {
        long Id { get; }

        long RepositoryId { get; }

        byte[] Sha1 { get; }

        // byte[] Sha3 { get;}
        
        // Author

        string Message { get; }

        DateTime Created { get; }
    }
}