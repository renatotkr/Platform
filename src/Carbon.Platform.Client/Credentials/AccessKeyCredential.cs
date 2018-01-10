using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Security
{
    public class AccessKeyCredential : ICredential
    {
        public AccessKeyCredential(string accessKeyId, string accessKeySecret, long? accountId, string scope = null)
        {
            AccessKeyId     = accessKeyId ?? throw new ArgumentNullException(nameof(accessKeyId));
            AccessKeySecret = accessKeySecret ?? throw new ArgumentNullException(nameof(accessKeySecret));
            AccountId       = accountId;
            Scope           = scope;
        }

        [DataMember(Name = "accessKeyId")]
        public string AccessKeyId { get; }

        [DataMember(Name = "accessKeySecret")]
        public string AccessKeySecret { get; }

        [DataMember(Name = "accountId")]
        public long? AccountId { get; }

        [DataMember(Name = "scope")]
        public string Scope { get; }
    }
}