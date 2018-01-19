using System;
using System.Net;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Security;

namespace Carbon.Cloud.Logging
{
    [Dataset("Clients", Schema = "Logs")]
    [DataContract]
    public class Client : IClient
    {
        public Client() { }

        public Client(Uid id, IPAddress ip, string userAgent)
        {
            Ensure.NotNull(ip, nameof(ip));
            Ensure.NotNull(userAgent, nameof(userAgent));

            Id        = id;
            Ip        = ip;
            UserAgent = userAgent;
            Hash      = ClientHash.Compute(this); // sha1(id, ip, userAgent)
        }

        [Member("id"), Key]
        [DataMember(Name = "id", Order = 1)]
        public Uid Id { get; }
        
        [Member("hash"), Key]
        [FixedSize(ClientHash.Length)]
        [DataMember(Name = "hash", Order = 2)]
        public byte[] Hash { get; }
        
        [Member("ip")]
        [DataMember(Name = "ip", Order = 3)]
        public IPAddress Ip { get; }

        [Member("userAgent")]
        [StringLength(1000)]
        [DataMember(Name = "userAgent", Order = 3)]
        public string UserAgent { get; }

        [Member("location")]
        [StringLength(255)]
        [DataMember(Name = "location", Order = 4, EmitDefaultValue = false)]
        public string Location { get; set; }

        #region IClient

        string IClient.Id => Id.ToString();

        #endregion
    }
}
