using System;
using System.Threading.Tasks;

using Carbon.Data.Protection;

namespace Carbon.Kms.Services
{
    public class DataDecrypytor : IDataDecrypter
    {
        private readonly IDataProtectorProvider protectorProvider;

        public DataDecrypytor(IDataProtectorProvider protectorProvider)
        {
            this.protectorProvider = protectorProvider ?? throw new ArgumentNullException(nameof(protectorProvider));
        }

        public async ValueTask<byte[]> DecryptAsync(EncryptedMessage encryptedDataMessage)
        {
            #region Preconditions

            if (encryptedDataMessage == null)
                throw new ArgumentNullException(nameof(encryptedDataMessage));
            
            #endregion

            var protector = await protectorProvider.GetAsync(encryptedDataMessage.Header.KeyId).ConfigureAwait(false) as DataProtector;
            
            return await protector.DecryptAsync(encryptedDataMessage).ConfigureAwait(false);
        }

        public ValueTask<byte[]> DecryptAsync(byte[] data)
        {
            var message = Serializer.Deserialize<EncryptedMessage>(data);

            return DecryptAsync(message);
          
        }
    }
}
