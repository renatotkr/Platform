namespace Carbon.Kms
{
    public enum KeyDataFormat : byte
    {
        Unknown               = 0,
        EncryptedDataMessage  = 1, // Protobuf encoded EncryptedMessage
        RsaPrivateKey         = 2, // PKCS#1 DER encoded
        RsaPublicKey          = 3, // PKCS#1 DER encoded

        /// <summary>
        /// http://docs.aws.amazon.com/encryption-sdk/latest/developer-guide/message-format.html#data-key-provider-id-length
        /// </summary>
        AwsKmsEncryptedData   = 10, 
    }
}