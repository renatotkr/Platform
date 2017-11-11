using System.Threading.Tasks;

using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public interface ICertificateManager
    {
        Task<ICertificate> GetAsync(long id);

        Task<ICertificate> CreateAsync(CreateCertificateRequest request);

        Task<ICertificate> FindAsync(Hash fingerprint);

        Task DeleteAsync(ICertificate certificate);
    }
}