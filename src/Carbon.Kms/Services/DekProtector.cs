using System;

using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public class DekProtector : IDekProtector
    {
        private readonly IDekProvider dekProvider;

        public DekProtector(IDekProvider dekProvider)
        {
            this.dekProvider = dekProvider ?? throw new ArgumentNullException(nameof(dekProvider));
        }

        public EncryptedData Encrypt(long keyId, byte[] plaintext)
        {
            var key = dekProvider.GetAsync(keyId).Result;

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
            var dek = dekProvider.GetAsync(data.KeyId, data.KeyVersion).Result;

            using (var aes = new AesDataProtector(dek.Value, data.IV))
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
