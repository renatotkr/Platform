using System.Threading.Tasks;

using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public interface IDataDecryptor
    {
        ValueTask<byte[]> DecryptAsync(byte[] data);

        ValueTask<byte[]> DecryptAsync(EncryptedDataMessage data);
    }
}