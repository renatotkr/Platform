using System.Threading.Tasks;

namespace Carbon.Kms
{
    public interface ISecretStore
    {
        Task AddAsync(SecretInfo secret);

        Task RemoveAsync(SecretInfo secret);

        Task<SecretInfo> GetAsync(long id);

        Task<SecretInfo> FindAsync(long ownerId, string name);
    }
}
