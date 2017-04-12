using System;

using Carbon.Platform.Resources;

namespace Carbon.Platform.VersionControl
{
    public interface IRepositoryCommit : IResource
    {
        long RepositoryId { get; }

        byte[] Sha1 { get; }

        string Message { get; }

        DateTime Created { get; }
    }
}

// FUTURE: SHA3