using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public interface IDekProtector
    {
        byte[] Decrypt(EncryptedData data);

        EncryptedData Encrypt(string keyId, byte[] plaintext);
    }
}