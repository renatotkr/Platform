namespace Carbon.Platform.Hosting
{
    public enum DomainRecordType
    {
        A     = 1,
        AAAA  = 28,
        CAA   = 257,
        CNAME = 5,
        LOC   = 29,
        MX    = 15,
        NAPTR = 35,
        NS    = 2,
        PTR   = 12,
        SOA   = 6,
        SPF   = 99,
        SVR   = 33,
        TXT   = 16,

        // Private use 65280-65534
        SVR2 = 65280 // environment?
    }
}

// https://en.wikipedia.org/wiki/List_of_DNS_record_types