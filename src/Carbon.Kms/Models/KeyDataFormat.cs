namespace Carbon.Kms
{
    public enum KeyDataFormat : byte
    {
        Unknown               = 0,
        EncryptedDataMessage  = 1, // protobuf serialized EncryptedMessage
        Ans1EncodedPrivateKey = 2,
        Ans1EncodedPublicKey  = 3,

        /// <summary>
        /// http://docs.aws.amazon.com/encryption-sdk/latest/developer-guide/message-format.html#data-key-provider-id-length
        /// </summary>
        AwsKmsEncryptedData           = 10, 
    }
}