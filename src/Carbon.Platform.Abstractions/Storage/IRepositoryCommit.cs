using System;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public interface IRepositoryCommit : IResource
    {
        long RepositoryId { get; }

        byte[] Sha1 { get; }

        // byte[] Sha3 { get;}

        string Message { get; }

        DateTime Created { get; }
    }
}