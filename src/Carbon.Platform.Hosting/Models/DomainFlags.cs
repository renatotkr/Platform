namespace Carbon.Platform.Hosting
{
    public enum DomainFlags
    {
        None      = 0,
        Tld       = 1 << 0, // com, org, net, google
        Sld       = 1 << 1, // second level TLD (co.uk)
        Managed   = 1 << 5, // provided through a register (top most registrable domain)
        Authoritative = 1 << 6, // Authoritive zone
        Hsts      = 1 << 7, // HTTP Strict Transport Security
        AutoRenew = 1 << 10
    }

    // Subdomain?
}