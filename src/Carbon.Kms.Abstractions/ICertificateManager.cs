using System.Threading.Tasks;

namespace Carbon.Kms
{
    public interface ICertificateManager
    {
        Task<ICertificate> CreateAsync(CreateCertificateRequest request);

        Task DeleteAsync(ICertificate certificate);
    }
}