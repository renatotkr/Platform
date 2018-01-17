using System;

namespace Carbon.Platform.Storage
{
    public class GetObjectPropertiesRequest
    {
        public GetObjectPropertiesRequest(string bucketName, string key)
        {
            BucketName  = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
            Key         = key ?? throw new ArgumentNullException(nameof(key));
        }

        public string BucketName { get; }

        public string Key { get; }
    }
}
