using System;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Hosting
{
    public interface ICertificate : IManagedResource
    {
        string[] Subjects { get; }

        DateTime? Issued { get; }

        DateTime Expires { get; }

        DateTime? Revoked { get; }
    }
}


// Google   ulong       compute#sslCertificate