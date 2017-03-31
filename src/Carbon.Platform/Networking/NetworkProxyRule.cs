using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Networking
{
    using Data.Annotations;

    [Dataset("NetworkProxyRules")]
    public class NetworkProxyRule : INetworkProxyRule
    {
        // proxyId + sequenceNumber
        [Member("id"), Key]
        public long Id { get; set; }

        // path matches '/' || path matches '/status'

        [Member("condition")]
        public string Condition { get; set; }

        // forward -> backend#100
        // arn:aws:elasticloadbalancing:ua-west-2:123456789012:targetgroup/my-targets/73e2d6bc24d8a067
        [Member("action")]
        public string Action { get; set; }

        [Member("priority")]
        public int Priority { get; set; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        public long ProxyId => ScopedId.GetScope(Id);

        #region Provider

        // aws
        // 50dc6c495c0c9188/f2f7dc8efc522ab2/9683b2d02a6cabee

        // full ARN?

        [Member("providerId")]
        public int ProviderId { get; set; }

        // subnet-8360a9e7
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        #endregion
    }
}