using System;

namespace Carbon.Platform.Security
{
    public interface IEncryptionKey
    {
        long Id { get; }
      
        string Name { get; }

        int Version { get; }

        DateTime? NextRotation { get; }

        long LocationId { get; }
    }
}


/*
Amazon   KMS         https://aws.amazon.com/kms/
Azure    Key Vault   https://azure.microsoft.com/en-us/services/key-vault/
Google   KMS         https://cloud.google.com/kms/
*/
