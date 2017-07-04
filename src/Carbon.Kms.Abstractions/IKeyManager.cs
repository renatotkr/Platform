using System.Threading.Tasks;

using Carbon.Data.Sequences;

namespace Carbon.Kms
{
    public interface IKeyManager
    {
        Task<IKeyInfo> GenerateAsync(GenerateKeyRequest request);

        Task DeactivateAsync(Uid keyId);

        Task DestroyAsync(Uid keyId);
    }
}