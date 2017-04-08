using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Networking
{
    using Data.Annotations;

    [Dataset("NetworkProxyRules")]
    public class NetworkProxyRule : INetworkProxyRule
    {
        public NetworkProxyRule() { }

        public NetworkProxyRule(long id)
        {
            Id = id;
        }

        // networkProxyId + sequenceNumber
        [Member("id"), Key]
        public long Id { get; }

        [Member("condition")]
        [StringLength(500)]
        public string Condition { get; set; }

        [Member("action")]
        public string Action { get; set; }

        [Member("priority")]
        public int Priority { get; set; }

        public long NetworkProxyId => ScopedId.GetScope(Id);

        #region Provider

        // 50dc6c495c0c9188/f2f7dc8efc522ab2/9683b2d02a6cabee

        // full ARN?

        [Member("providerId")]
        public int ProviderId { get; set; }

        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #endregion

    }
}

// path matches '/' || path matches '/status'

// forward -> backend#100
// arn:aws:elasticloadbalancing:ua-west-2:123456789012:targetgroup/my-targets/73e2d6bc24d8a067