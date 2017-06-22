using System;

using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public class DataProtector : IDataProtector
    {
        private readonly IKeyProvider keyProvider;

        public DataProtector(IKeyProvider keyProvider)
        {
            this.keyProvider = keyProvider ?? throw new ArgumentNullException(nameof(keyProvider));
        }

        public EncryptedData Encrypt(long keyId, byte[] plaintext)
        {
            var key = keyProvider.GetAsync(keyId).Result;

            var iv = Secret.Generate(16); // 128 bit iv

            using (var aes = new AesDataProtector(key: key.Value, iv: iv.Value))
            {
                var ciphertext = aes.Encrypt(plaintext);

                return new EncryptedData(
                    keyId      : key.Id,
                    keyVersion : key.Version,
                    iv         : iv.Value,
                    ciphertext : ciphertext
                );
            }
        }

        public byte[] Decrypt(EncryptedData data)
        {
            var key = keyProvider.GetAsync(long.Parse(data.KeyId), data.KeyVersion).Result;

            using (var aes = new AesDataProtector(key.Value, data.IV))
            {
                return aes.Decrypt(data.Ciphertext);
            }
        }
    }
}

/*
DEK: Data Encryption Key 
KEK: Key Encryption Key 
*/
