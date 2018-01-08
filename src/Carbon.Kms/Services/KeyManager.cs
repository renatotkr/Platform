using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Carbon.Data.Sequences;
using Carbon.Json;
using Carbon.Time;

namespace Carbon.Kms.Services
{
    public class KeyManager : IKeyManager
    {
        private readonly Uid masterKeyId;
        private readonly IKeyStore keyStore;
        private readonly IClock clock;
        private readonly IDataProtectorProvider protectorProvider;

        public KeyManager(
            IClock clock,
            IKeyStore keyStore, 
            Uid masterKeyId,
            IDataProtectorProvider protectorProvider)
        {
            this.clock             = clock             ?? throw new ArgumentNullException(nameof(clock));
            this.keyStore          = keyStore          ?? throw new ArgumentNullException(nameof(keyStore));
            this.masterKeyId       = masterKeyId;
            this.protectorProvider = protectorProvider ?? throw new ArgumentNullException(nameof(protectorProvider));
        }

        public async Task<IKeyInfo> GenerateAsync(GenerateKeyRequest request)
        {
            Validate.NotNull(request, nameof(request));

            var aes = Aes.Create();
            
            // keying material.

            aes.KeySize = 256;
            aes.GenerateKey();

            var masterKey = await protectorProvider.GetAsync(masterKeyId.ToString(), request.Aad).ConfigureAwait(false);

            var ciphertext = await masterKey.EncryptAsync(aes.Key).ConfigureAwait(false);

            Uid id = Guid.NewGuid();

            var key = new KeyInfo(
                id          : id,
                ownerId     : request.OwnerId,
                name        : request.Name ?? id.ToString(),
                kekId       : masterKeyId,
                format      : KeyDataFormat.AwsKmsEncryptedData,
                data        : ciphertext,
                activated   : DateTime.UtcNow.AddMinutes(-5),
                type        : request.Type,
                aad         : ToJson(request.Aad)
            );

            await keyStore.CreateAsync(key).ConfigureAwait(false);

            return key;
        }

        public async Task DeactivateAsync(Uid id)
        {
            await keyStore.DeactivateAsync(id);
        }

        public async Task DestroyAsync(Uid id)
        {
            await keyStore.DeleteAsync(id);
        }

        #region Helpers

        private static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(JsonObject json)
        {
            foreach (var property in json)
            {
                yield return new KeyValuePair<string, string>(property.Key, property.Value.ToString());
            }
        }

        private static JsonObject ToJson(IEnumerable<KeyValuePair<string, string>> pairs)
        {
            if (pairs == null) return null;

            var json = new JsonObject();

            foreach (var property in pairs)
            {
                json.Add(property.Key, property.Value);
            }

            return json;
        }

        #endregion
    }
}