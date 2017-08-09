using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Security
{
    public class AccessKeyCredential : ICredential
    {
        public AccessKeyCredential(string accessKeyId, string accessKeySecret, long? accountId)
        {
            AccessKeyId     = accessKeyId ?? throw new ArgumentNullException(nameof(accessKeyId));
            AccessKeySecret = accessKeySecret ?? throw new ArgumentNullException(nameof(accessKeySecret));
            AccountId       = accountId;
        }

        [DataMember(Name = "accessKeyId")]
        public string AccessKeyId { get; }

        [DataMember(Name = "accessKeySecret")]
        public string AccessKeySecret { get; }

        [DataMember(Name = "accountId")]
        public long? AccountId { get; }
    }
}

// Support self signed JWT tokens from 