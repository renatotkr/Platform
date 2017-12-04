namespace Carbon.Platform.Hosting
{
    public enum DomainContactType
    {
        Unknown       = 0,
        Registrant    = 1 << 0, // Owner
        Administrator = 1 << 1, // Admin
        Technical     = 1 << 2, // Tech
        Billing       = 1 << 3
    }
}