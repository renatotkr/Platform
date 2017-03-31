using System.Runtime.Serialization;

namespace Carbon.Platform
{
    [DataContract]
    public class AppStats
    {
        [DataMember(Order = 1)]
        public long AppId { get; set; }

        [DataMember(Order = 2)]
        public long RequestCount { get; set; }

        [DataMember(Order = 3)]
        public long ErrorCount { get; set; }

        // BytesSent
        // BytesRecieved
    }
}