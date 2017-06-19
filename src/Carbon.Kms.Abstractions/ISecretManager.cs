using System;
using System.Threading.Tasks;

namespace Carbon.Kms
{
    public interface ISecretManager
    {
        Task CreateAsync(string name, byte[] value, string keyId, DateTime? expires = null);

        Task<byte[]> DecryptAsync(string name);

        Task DeleteAsync(long id);
    }
}