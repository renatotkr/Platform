namespace Carbon.Kms
{
    public enum CertificateRevocationReason
    {
        Unspecified          = 0,
        KeyCompromise        = 1,
        CACompromise         = 2,
        AffilationChanged    = 3,
        Superseded           = 4,
        CessationOfOperation = 5,
        CertificateHold      = 6, 
        RevokeFromCLR        = 8,
        PrivilegeWithdrawn  = 9,
        AACompromise        = 10
    }
}