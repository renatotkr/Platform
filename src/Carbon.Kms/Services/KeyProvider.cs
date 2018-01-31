using System;
using System.Threading.Tasks;

using Carbon.Data.Protection;
using Carbon.Data.Sequences;

using Carbon.Time;

namespace Carbon.Kms.Services
{
    public class KeyProvider : IKeyProvider
    {
        private readonly IClock clock;
        private readonly IDataProtectorProvider protectorFactory;
        private readonly IKeyStore keyStore;

        public KeyProvider(
            IClock clock,
            IDataProtectorProvider protectorProvider,
            IKeyStore keyStore)
        {
            this.clock            = clock             ?? throw new ArgumentNullException(nameof(clock));
            this.keyStore         = keyStore          ?? throw new ArgumentNullException(nameof(keyStore));
            this.protectorFactory = protectorProvider ?? throw new ArgumentNullException(nameof(protectorProvider));
        }

        public async ValueTask<CryptoKey> GetAsync(long ownerId, string name)
        {
            Ensure.IsValidId(ownerId,   nameof(ownerId));
            Ensure.NotNullOrEmpty(name, nameof(name));

            var key = await keyStore.GetAsync(ownerId, name);

            return await DecryptAsync(key);
        }

        public async ValueTask<CryptoKey> GetAsync(Uid id)
        {
            var key = await keyStore.GetAsync(id);

            return await DecryptAsync(key);
        }

        private async Task<CryptoKey> DecryptAsync(KeyInfo key)
        {
            Ensure.NotNull(key, nameof(key));

            switch (key.Status)
            {
                case KeyStatus.Deactivated : throw new Exception($"key#{key.Id} is deactivated");
                case KeyStatus.Compromised : throw new Exception($"key#{key.Id} was compromised and may not longer be used");
                case KeyStatus.Destroyed   : throw new Exception($"key#{key.Id} is destroyed");
                case KeyStatus.Suspended   : throw new Exception($"key#{key.Id} is suspended");
            }

            if (key.Expires != null && key.Expires <= clock.Observe())
            {
                throw new KeyExpiredException(key, key.Expires.Value);
            }

            var kek = await protectorFactory.GetAsync(
                keyId : key.KekId.ToString(),
                aad   : key.GetAad()
            );

            // use the key encryption key to decrypt it
            var result = await kek.DecryptAsync(key.Data);

            return new CryptoKey(
                id    : key.Id.ToString(),
                value : result
            );
        }
    }
}
