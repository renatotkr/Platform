namespace Carbon.Platform.Hosting
{
    public enum DomainContactType
    {
        Unknown       = 0,
        Registrant    = 1 << 0,
        Administrator = 1 << 1,
        Technical     = 1 << 2
    }
}