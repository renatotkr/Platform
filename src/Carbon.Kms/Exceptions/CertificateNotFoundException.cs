using System;

namespace Carbon.Kms
{
    public class CertificateNotFoundException : Exception
    {
        public CertificateNotFoundException(long certificateId)
            : base($"borg:certificate/{certificateId} not found") { }
    }
}