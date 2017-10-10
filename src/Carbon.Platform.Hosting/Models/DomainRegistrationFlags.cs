namespace Carbon.Platform.Hosting
{
    public enum DomainRegistrationFlags
    {
        None    = 0,
        Managed = 1 << 0,
        Private = 1 << 1
    }
}