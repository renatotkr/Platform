using System;
using System.Threading.Tasks;

using Carbon.Data.Protection;

namespace Carbon.Kms
{
    public class DataDecryptor : IDataDecryptor
    {
        private readonly IDataProtectorProvider protectorProvider;

        public DataDecryptor(IDataProtectorProvider protectorProvider)
        {
            this.protectorProvider = protectorProvider ?? throw new ArgumentNullException(nameof(protectorProvider));
        }

        public async ValueTask<byte[]> DecryptAsync(EncryptedDataMessage message)
        {
            Ensure.NotNull(message, nameof(message));

            var protector = await protectorProvider.GetAsync(message.Header.KeyId).ConfigureAwait(false) as DataProtector;

            return protector.Decrypt(message);
        }

        public ValueTask<byte[]> DecryptAsync(byte[] data)
        {
            Ensure.NotNullOrEmpty(data, nameof(data));

            var message = Serializer.Deserialize<EncryptedDataMessage>(data);

            return DecryptAsync(message);
        }
    }
}