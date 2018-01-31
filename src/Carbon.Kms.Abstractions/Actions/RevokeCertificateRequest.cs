namespace Carbon.Kms
{
    public class RevokeCertificateRequest
    {
        public RevokeCertificateRequest(
            long id,
            CertificateRevocationReason reason = CertificateRevocationReason.Unspecified)
        {
            Ensure.IsValidId(id);

            Id     = id;
            Reason = reason;
        }

        public long Id { get; }

        public CertificateRevocationReason Reason { get; }
    }
}