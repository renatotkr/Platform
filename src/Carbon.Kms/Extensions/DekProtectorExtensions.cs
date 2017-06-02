using System.Text;
using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public static class DekProtectorExtensions
    {
        public static EncryptedData EncryptString(
            this IDekProtector protector,
            long keyId,
            string plaintext)
        {
            var data = Encoding.UTF8.GetBytes(plaintext);

            return protector.Encrypt(keyId, data);
        }

        public static string DecryptString(
            this IDekProtector protector,
            EncryptedData data)
        {
            var plaintext = protector.Decrypt(data);

            return Encoding.UTF8.GetString(plaintext);
        }
    }
}