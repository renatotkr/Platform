using System;

namespace Carbon.Kms
{
    public interface ICertificate
    {
        long Id { get; }

        long OwnerId { get; }

        byte[] Data { get; }

        DateTime Issued { get; }

        DateTime Expires { get; }

        DateTime? Revoked { get; }
    }
}

// aws | arn   | Certificate
// gcp | ulong | compute#sslCertificate