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
            #region Preconditions

            if (message == null)
                throw new ArgumentNullException(nameof(message));
            
            #endregion

            var protector = await protectorProvider.GetAsync(message.Header.KeyId).ConfigureAwait(false) as DataProtector;

            return protector.Decrypt(message);
        }

        public ValueTask<byte[]> DecryptAsync(byte[] data)
        {
            #region Preconditions

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            #endregion

            var message = Serializer.Deserialize<EncryptedDataMessage>(data);

            return DecryptAsync(message);
        }
    }
}