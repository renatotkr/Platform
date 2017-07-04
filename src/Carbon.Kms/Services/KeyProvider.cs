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
        private readonly IDataProtectorProvider protectorProvider;
        private readonly IKeyStore keyStore;

        public KeyProvider(
            IClock clock,
            IDataProtectorProvider protectorProvider,
            IKeyStore keyService)
        {
            this.keyStore          = keyService        ?? throw new ArgumentNullException(nameof(keyService));
            this.clock             = clock             ?? throw new ArgumentNullException(nameof(clock));
            this.protectorProvider = protectorProvider ?? throw new ArgumentNullException(nameof(protectorProvider));
        }   

        public async ValueTask<CryptoKey> GetAsync(Uid id)
        {
            var key = await keyStore.GetAsync(id);

            if (key.Expires != null && key.Expires <= clock.Observe())
            {
                throw new Exception($"key expired on '{key.Expires}' and may not be used");
            }

            var dek = await protectorProvider.GetAsync(
                keyId : key.KekId.ToString(), 
                aad   : key.GetAad()
            ).ConfigureAwait(false);

            var result = await dek.DecryptAsync(key.Data).ConfigureAwait(false);

            return new CryptoKey(
                id    : key.Id.ToString(),
                value : result
            );
        }
    }
}
