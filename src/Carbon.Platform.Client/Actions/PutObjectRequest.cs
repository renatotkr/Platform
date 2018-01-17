using System;
using System.Collections.Generic;
using System.IO;

namespace Carbon.Platform.Storage
{
    public class PutObjectRequest
    {
        public PutObjectRequest(string bucketName, string key, Stream stream, IReadOnlyDictionary<string, string> properties = null)
        {
            BucketName  = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
            Key         = key ?? throw new ArgumentNullException(nameof(key));
            Stream      = stream ?? throw new ArgumentNullException(nameof(stream));
            Properties  = properties;
        }

        public string BucketName { get; }

        public string Key { get; }

        public Stream Stream { get; }

        public IReadOnlyDictionary<string, string> Properties { get; }
    }
}
