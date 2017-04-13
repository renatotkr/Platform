using System.Runtime.Serialization;

using Carbon.Protection;

namespace Carbon.Platform
{
    using Apps;

    public class AppDetails : IApp
    {
        [DataMember(Name = "id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "name", Order = 1)]
        public string Name { get; set; }

        [DataMember(Name = "type", Order = 3)]
        public AppType Type { get; set; }

        [DataMember(Name = "digest", Order = 5)]
        public Hash Digest { get; set; }
    }
}
