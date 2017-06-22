namespace Carbon.Kms
{
    public interface IKeyInfo
    {
        string Id { get; }

        int Version { get; }

        // Type
    }
}

// master keying material 
// derive DEKs/CEKs

// Google KMS: CryptoKey
// $0.06 per key per month
// stored within a "KeyRing"

// Azure: HSM Protected keys
// $1/m

// The actual key never leaves the device...

// CryptoKey ?

/*
aws   | KMS       | https://aws.amazon.com/kms/
azure | Key Vault | https://azure.microsoft.com/en-us/services/key-vault/
gcp   | KMS       | https://cloud.google.com/kms/
*/
