using System;

namespace Carbon.Kms
{
    public class CertificateNotFoundException : Exception
    {
        public CertificateNotFoundException(long certificateId)
            : base($"certificate#{certificateId} does not exist") { }
    }
}
