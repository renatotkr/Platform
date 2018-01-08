namespace Carbon.Kms
{
    public class RevokeCertificateRequest
    {
        public RevokeCertificateRequest(long id, CertificateRevocationReason reason)
        {
            Id = id;
            Reason = reason;
        }

        public long Id { get; }

        public CertificateRevocationReason Reason { get; }
    }
}