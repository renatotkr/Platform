using System;
using Carbon.Data.Sequences;

namespace Carbon.Kms
{
    public interface ICertificate // : IResource
    {
        Uid Id { get; }

        string Subject { get; }
        
        long? IssuerId { get; }

        DateTime? Issued { get; }

        DateTime? Expires { get; }

        DateTime? Revoked { get; }
    }
}

// aws | arn   | Certificate
// gcp | ulong | compute#sslCertificate

/*
Certificate
Version Number
Serial Number
Signature Algorithm ID
Issuer Name
Validity period
Not Before
Not After
Subject name
Subject Public Key Info
Public Key Algorithm
Subject Public Key
Extensions (optional)
*/
