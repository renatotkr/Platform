using System;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    using static Expression;

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
            var bucket = new BucketInfo(
                id       : await db.Buckets.Sequence.NextAsync(),
                name     : request.Name,
                ownerId  : request.OwnerId,
                resource : request.Resource
            );

            await db.Buckets.InsertAsync(bucket);

            return bucket;
        }

        public async Task<bool> DeleteAsync(IBucketInfo bucket)
        {
            return await db.Buckets.PatchAsync(bucket.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}