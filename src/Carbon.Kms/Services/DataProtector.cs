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

        public byte[] Encrypt(byte[] plaintext)
        {
            Validate.NotNullOrEmpty(plaintext, nameof(plaintext));

            var iv = Secret.Generate(16); // 128 bit iv

            using (var aes = new AesDataProtector(key: key.Value, iv: iv.Value))
            {
                var ciphertext = aes.Encrypt(plaintext);

                var message = new EncryptedDataMessage(
                    keyId      : key.Id,
                    iv         : iv.Value,
                    ciphertext : ciphertext
                );

                return Serializer.Serialize(message);
            }
        }

        public byte[] Decrypt(byte[] data)
        {
            Validate.NotNullOrEmpty(data, nameof(data));

            var message = Serializer.Deserialize<EncryptedDataMessage>(data);
            
            return Decrypt(message);
        }

        public byte[] Decrypt(EncryptedDataMessage message)
        {
            Validate.NotNull(message, nameof(message));
            
            if (message.Ciphertext == null || message.Ciphertext.Length == 0)
                throw new ArgumentException("Required", "ciphertext");

            if (message.Header.KeyId != key.Id)
                throw new Exception($"message key '{message.Header.KeyId}' does not match protector");

            using (var aes = new AesDataProtector(key.Value, message.IV))
            {
                return aes.Decrypt(message.Ciphertext);
            }
        }

        #region IDataProtector

        ValueTask<byte[]> IDataProtector.DecryptAsync(byte[] ciphertext)
        {
            return new ValueTask<byte[]>(Decrypt(ciphertext));
        }

        ValueTask<byte[]> IDataProtector.EncryptAsync(byte[] plaintext)
        {
            return new ValueTask<byte[]>(Encrypt(plaintext));
        }

        #endregion
    }
}