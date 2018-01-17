using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Storage;

namespace Carbon.Platform.Storage
{
    public class Container : IBucket
    {
        private readonly string name;
        private readonly StorageClient client;
        
        public Container(string name, StorageClient client)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.client = client ?? throw new ArgumentNullException(nameof(name));
        }

        public async Task<IBlob> GetAsync(string key)
        {
            return await GetAsync(key, new GetBlobOptions());
        }

        public async Task<IBlobResult> GetAsync(string key, GetBlobOptions options)
        {
            Ensure.NotNullOrEmpty(key, nameof(key));
            
            // TODO: Support blob options

            return await client.GetObjectAsync(new GetObjectRequest(name, key));
        }

        public Task<IReadOnlyDictionary<string, string>> GetPropertiesAsync(string key)
        {
            Ensure.NotNullOrEmpty(key, nameof(key));

            return client.GetObjectPropertiesAsync(new GetObjectPropertiesRequest(name, key));
        }

        public Task PutAsync(IBlob blob)
        {
            return PutAsync(blob, null);
        }

        public async Task PutAsync(IBlob blob, PutBlobOptions options)
        {
            Ensure.NotNull(blob, nameof(blob));

            if (options?.EncryptionKey != null)
            {
                throw new Exception("Encryption keys not yet supported");
            }

            var stream = await blob.OpenAsync();

            await client.PutObjectAsync(new PutObjectRequest(name, blob.Key, stream, blob.Properties));
        }

        public Task DeleteAsync(string key)
        {
            Ensure.NotNullOrEmpty(key, nameof(key));

            return client.DeleteObjectAsync(new DeleteObjectRequest(name, key));
        }
    
        public Task<IReadOnlyList<IBlob>> ListAsync(string prefix = null)
        {
            return client.ListBucketAsync(new ListBucketRequest(name, prefix));
        }
    }
}