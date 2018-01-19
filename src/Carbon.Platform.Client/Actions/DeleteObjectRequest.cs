namespace Carbon.Platform.Storage
{
    public class DeleteObjectRequest
    {
        public DeleteObjectRequest(string bucketName, string key)
        {
            Ensure.NotNullOrEmpty(bucketName, nameof(bucketName));
            Ensure.NotNull(key,               nameof(key));

            BucketName = bucketName;
            Key = key;
        }

        public string BucketName { get; }

        public string Key { get; }
    }
}