﻿using Carbon.Platform.Resources;

namespace Carbon.Platform.Data
{
    public interface IEncryptionKey : IManagedResource
    {      
        string Name { get; }
    }
}


/*
Amazon   KMS         https://aws.amazon.com/kms/
Azure    Key Vault   https://azure.microsoft.com/en-us/services/key-vault/
Google   KMS         https://cloud.google.com/kms/
*/