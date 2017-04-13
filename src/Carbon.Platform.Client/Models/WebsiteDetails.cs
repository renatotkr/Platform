using System;
using System.Runtime.Serialization;

namespace Carbon.Platform
{
    public class WebsiteDetails
    {
        [DataMember(Name = "id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "name", Order = 2)]
        public string Name { get; set; }

        [DataMember(Name = "revision", Order = 3)]
        public string Revision { get; set; }

        [DataMember(Name = "modified", Order = 6)]
        public DateTime Modified { get; set; }
    }
}
