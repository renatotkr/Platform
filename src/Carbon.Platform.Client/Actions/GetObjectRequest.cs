namespace Carbon.Platform.Storage
{
    public class GetObjectRequest
    {
        public GetObjectRequest(string bucketName, string key)
        {
            Ensure.NotNullOrEmpty(bucketName, nameof(bucketName));
            Ensure.NotNullOrEmpty(key, nameof(key));

            BucketName = bucketName;
            Key        = key;
        }

        public string BucketName { get; }

        public string Key { get; }
    }
}
