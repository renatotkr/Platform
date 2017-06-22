using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Kms
{
    public interface IKeyProtector
    {
        ValueTask<byte[]> DecryptAsync(
            byte[] ciphertext,
            IEnumerable<KeyValuePair<string, string>> additionalAuthenticatedData
        );

        ValueTask<byte[]> EncryptAsync(
           byte[] plaintext,
           IEnumerable<KeyValuePair<string, string>> additionalAuthenticatedData
        );
    }
}

// Does the KMS device generate better key source material than .NET?