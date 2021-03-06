﻿namespace Carbon.Platform.Storage
{
    public class ListBucketRequest
    {
        public ListBucketRequest(string bucketName, string prefix = null)
        {
            Ensure.NotNullOrEmpty(bucketName, nameof(bucketName));

            BucketName = bucketName;
            Prefix = prefix;
        }

        public string BucketName { get; }

        public string Prefix { get; }
    }
}