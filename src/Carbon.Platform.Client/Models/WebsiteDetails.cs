using System;
using System.Runtime.Serialization;

using Carbon.Versioning;

namespace Carbon.Platform.Web
{
    public class WebsiteDetails : IWebsite
    {
        [DataMember(Name = "id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "ownerId", Order = 2)]
        public long OwnerId { get; set; }

        [DataMember(Name = "name", Order = 3)]
        public string Name { get; set; }

        [DataMember(Name = "version", Order = 4)]
        public SemanticVersion Version { get; set; }

        [DataMember(Name = "modified", Order = 5)]
        public DateTime Modified { get; set; }

        [DataMember(Name = "package", Order = 5)]
        public PackageInfo Package { get; set; }
    }
}