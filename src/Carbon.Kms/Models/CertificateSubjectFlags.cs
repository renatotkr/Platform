namespace Carbon.Kms
{
    public enum CertificateSubjectFlags
    {
        None      = 0,
        Primary   = 1 << 0,
        Alternate = 1 << 1
    }
}