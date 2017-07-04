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

            #endregion

            var iv = Secret.Generate(16); // 128 bit iv

            using (var aes = new AesDataProtector(key: key.Value, iv: iv.Value))
            {
                var ciphertext = aes.Encrypt(plaintext);

                var message = new EncryptedMessage(
                    keyId      : key.Id,
                    iv         : iv.Value,
                    ciphertext : ciphertext
                );

                var encryptedMessageData = Serializer.Serialize(message);

                return new ValueTask<byte[]>(encryptedMessageData);
            }
        }

        public ValueTask<byte[]> DecryptAsync(byte[] ciphertext)
        {
            #region Preconditions

            if (ciphertext == null)
                throw new ArgumentNullException(nameof(ciphertext));

            #endregion

            var message = Serializer.Deserialize<EncryptedMessage>(ciphertext);
            
            return DecryptAsync(message);
        }

        public ValueTask<byte[]> DecryptAsync(EncryptedMessage message)
        {
            #region Preconditions

            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (message.Header.KeyId != key.Id)
                throw new Exception("wrong key:" + message.Header.KeyId);

            #endregion

            using (var aes = new AesDataProtector(key.Value, message.IV))
            {
                return new ValueTask<byte[]>(aes.Decrypt(message.Ciphertext));
            }
        }
    }
}