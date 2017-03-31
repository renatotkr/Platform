using System;

namespace Carbon.Platform.Certificates
{
    public interface ICertificate
    {
        long Id { get; }

        string[] Subjects { get; }

        DateTime? Issued { get; }

        DateTime Expires { get; }

        DateTime? Revoked { get; }

        int ProviderId { get; }
    }
}