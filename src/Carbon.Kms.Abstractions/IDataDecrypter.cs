using System.Threading.Tasks;

namespace Carbon.Kms
{
    public interface IDataDecryptor
    {
        ValueTask<byte[]> DecryptAsync(byte[] data); // encryptedMessage?
    }
}