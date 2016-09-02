using System;

namespace Carbon.Networking
{
    using Data.Annotations;

    [Dataset("UrlRoutes")]
    public class UrlRoute
    {
        [Member(1), Key]
        public string Host { get; set; } // e.g. api.domain.com

        [Member(2), Key]
        public byte[] Hash { get; set; }
        
        [Member(3)]
        public string Path { get; set; }
        
        // public string EntryPoint { get ;set; } // e.g. FancyFunction

        [Member(4)]
        public int Order { get; set; }

        [Member(5)]
        public long BackendId { get; set; }
    }

    public class UrlHost
    {
        [Member(1), Key]
        public string Name { get; set; } // data.gov
     
        [Member(2), Mutable]
        public DateTime? Verified { get; set; }

        [Member(3), Mutable] 
        public long CertificateId { get; set; }
    }
}

// Terminates SSL & routes urls to backends based on hosts / urls