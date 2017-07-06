using System;
using System.Threading.Tasks;

using Carbon.Data.Protection;
using Carbon.Data.Sequences;

using Carbon.Time;

namespace Carbon.Kms
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
            var key = await keyStore.GetAsync(ownerId, name).ConfigureAwait(false);

            return await DecryptAsync(key).ConfigureAwait(false);
        }

        public async ValueTask<CryptoKey> GetAsync(Uid id)
        {
            var key = await keyStore.GetAsync(id).ConfigureAwait(false);

            return await DecryptAsync(key).ConfigureAwait(false);
        }

        private async Task<CryptoKey> DecryptAsync(KeyInfo key)
        {
            #region Preconditions

            switch (key.Status)
            {
                case KeyStatus.Deactivated : throw new Exception($"key#{key.Id} is deactivated");
                case KeyStatus.Compromised : throw new Exception($"key#{key.Id} was comprimised and may not longer be used");
                case KeyStatus.Destroyed   : throw new Exception($"key#{key.Id} is destroyed");
                case KeyStatus.Suspended   : throw new Exception($"key#{key.Id} is suspended");
            }

            if (key.Expires != null && key.Expires <= clock.Observe())
            {
                throw new Exception($"key#{key.Id} expired on '{key.Expires}'");
            }

            #endregion

            var kek = await protectorFactory.GetAsync(
                keyId: key.KekId.ToString(),
                aad: key.GetAad()
            ).ConfigureAwait(false);

            // use the key encryption key to decrypt it
            var result = await kek.DecryptAsync(key.Data).ConfigureAwait(false);

            return new CryptoKey(
                id: key.Id.ToString(),
                value: result
            );
        }
    }
}
