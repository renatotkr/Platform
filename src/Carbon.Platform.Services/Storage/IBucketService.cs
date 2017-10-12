using System.Threading.Tasks;

namespace Carbon.Platform.Storage
{
    public interface IBucketService
    {
        Task<BucketInfo> GetAsync(long id);

        Task<BucketInfo> RegisterAsync(RegisterBucketRequest request);

        Task<bool> DeleteAsync(IBucketInfo bucket);
    }
}