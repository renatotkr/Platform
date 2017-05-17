using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public interface IEncryptionKey : IManagedResource
    {      
        string Name { get; }
    }
}

/*
aws   | KMS       | https://aws.amazon.com/kms/
azure | Key Vault | https://azure.microsoft.com/en-us/services/key-vault/
gcp   | KMS       | https://cloud.google.com/kms/
*/
