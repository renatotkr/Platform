using System;
using System.Runtime.Serialization;

namespace Carbon.Platform
{
    public class WebsiteDetails
    {
        [DataMember(Name = "id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "ownerId", Order = 2)]
        public long OwnerId { get; set; }

        [DataMember(Name = "name", Order = 3)]
        public string Name { get; set; }

        [DataMember(Name = "modified", Order = 5)]
        public DateTime Modified { get; set; }
    }
}