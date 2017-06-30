using System;
using System.Net;

using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Security;

namespace Carbon.Cloud.Logging
{
    [Dataset("Clients", Schema = "Logging")]
    public class Client : IClient
    {
        public Client() { }

        public Client(Uid id, IPAddress ip, string userAgent)
        {
            Id        = id;
            Ip        = ip ?? throw new ArgumentNullException(nameof(ip));
            UserAgent = userAgent ?? throw new ArgumentNullException(nameof(userAgent));
            Hash      = ClientHash.Compute(this); // sha1(id, ip, userAgent)
        }
        // entropy | entropy
        // upper   | lower
        [Member("id"), Key]
        public Uid Id { get; }
        
        [Member("hash"), Key, FixedSize(ClientHash.Length)]
        public byte[] Hash { get; }
        
        [Member("ip")]
        public IPAddress Ip { get; }

        [Member("userAgent")]
        [StringLength(1000)]
        public string UserAgent { get; }

        #region IClient

        Guid IClient.Id => new Guid(Id.Serialize());

        #endregion
    }
}
