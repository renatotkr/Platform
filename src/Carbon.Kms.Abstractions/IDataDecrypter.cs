using System.Threading.Tasks;

using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public interface IDataDecrypter
    {
        ValueTask<byte[]> DecryptAsync(byte[] data);

        ValueTask<byte[]> DecryptAsync(EncryptedMessage data);
    }
}