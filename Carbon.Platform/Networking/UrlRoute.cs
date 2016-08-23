using Carbon.Data.Annotations;

namespace Carbon.Networking
{
    public class UrlRoute
    {
        [Member(1), Key]
        public string Host { get; set; } // e.g. api.domain.com

        [Member(2), Key]
        public byte[] Hash { get; set; }
        
        [Member(3)]
        public string Path { get; set; }

        [Member(4)]
        public int Order { get; set; }

        [Member(5)]
        public long BackendId { get; set; }

        [Member(6)]
        public long? CertificateId { get; set; }
    }
}

// Terminates SSL & routes urls to backends based on hosts / urls