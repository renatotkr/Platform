using System;
using System.Threading.Tasks;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public class BucketService : IBucketService
    {
        private readonly PlatformDb db;

        public BucketService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<BucketInfo> GetAsync(long id)
        {
            return await db.Buckets.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.Volume, id);
        }

        public async Task<BucketInfo> RegisterAsync(RegisterBucketRequest request)
        {
            #region Validation

            Validate.Object(request, nameof(request));

            #endregion

            var bucket = new BucketInfo(
                id       : await db.Buckets.Sequence.NextAsync(),
                name     : request.Name,
                ownerId  : request.OwnerId,
                resource : request.Resource
            );

            await db.Buckets.InsertAsync(bucket);

            return bucket;
        }
    }
}