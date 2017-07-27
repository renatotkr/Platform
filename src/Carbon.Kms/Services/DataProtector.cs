using System;
using System.Threading.Tasks;

using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public class DataProtector : IDataProtector
    {
        private readonly CryptoKey key;

        public DataProtector(CryptoKey key)
        {
            this.key = key;
        }

        public ValueTask<byte[]> EncryptAsync(byte[] plaintext)
        {
            #region Preconditions

            if (plaintext == null)
                throw new ArgumentNullException(nameof(plaintext));

            if (plaintext.Length == 0)
                throw new ArgumentException("May not be empty", nameof(plaintext));

            #endregion

            var iv = Secret.Generate(16); // 128 bit iv

            using (var aes = new AesDataProtector(key: key.Value, iv: iv.Value))
            {
                var ciphertext = aes.Encrypt(plaintext);

                var message = new EncryptedDataMessage(
                    keyId      : key.Id,
                    iv         : iv.Value,
                    ciphertext : ciphertext
                );

                var messageData = Serializer.Serialize(message);

                return new ValueTask<byte[]>(messageData);
            }
        }

        public ValueTask<byte[]> DecryptAsync(byte[] data)
        {
            #region Preconditions

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Length == 0)
                throw new ArgumentException("Must not be empty", nameof(data));
            #endregion

            var message = Serializer.Deserialize<EncryptedDataMessage>(data);
            
            return DecryptAsync(message);
        }

        public ValueTask<byte[]> DecryptAsync(EncryptedDataMessage message)
        {
            #region Preconditions

            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (message.Ciphertext == null || message.Ciphertext.Length == 0)
                throw new ArgumentException("Required", "ciphertext");

            if (message.Header.KeyId != key.Id)
                throw new Exception($"message key '{message.Header.KeyId}' does not match protector");

            #endregion

            using (var aes = new AesDataProtector(key.Value, message.IV))
            {
                return new ValueTask<byte[]>(aes.Decrypt(message.Ciphertext));
            }
        }
    }
}