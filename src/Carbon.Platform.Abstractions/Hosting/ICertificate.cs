using System;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Hosting
{
    public interface ICertificate : IManagedResource
    {
        string[] Subjects { get; }
        
        long IssuerId { get; }

        DateTime? Issued { get; }

        DateTime Expires { get; }

        DateTime? Revoked { get; }
    }
}

// aws | arn   | Certificate
// gcp | ulong | compute#sslCertificate