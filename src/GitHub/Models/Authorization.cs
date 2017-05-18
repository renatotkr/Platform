using System;
using System.Runtime.Serialization;

namespace GitHub
{
    public class Authorization
    {
        public long Id { get; set; }

        public string[] Scopes { get; set; }

        public string Token { get; set; }

        public string Note { get; set; }

        public string Fingerprint { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }
    }
}