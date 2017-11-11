using System;

namespace Carbon.Kms
{
    public interface ICertificate
    {
        long Id { get; }

        byte[] Data { get; }

        long? ParentId { get; }

        byte[] Fingerprint { get; } // sha256(data)

        DateTime Expires { get; }

        DateTime? Revoked { get; }
    }
}

// aws | arn   | Certificate
// gcp | ulong | compute#sslCertificate