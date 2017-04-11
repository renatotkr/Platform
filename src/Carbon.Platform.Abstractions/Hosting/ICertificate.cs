using System;

namespace Carbon.Platform.Hosting
{
    public interface ICertificate : IManagedResource
    {
        long Id { get; }

        string[] Subjects { get; }

        DateTime? Issued { get; }

        DateTime Expires { get; }

        DateTime? Revoked { get; }
    }
}


// Google   ulong       compute#sslCertificate