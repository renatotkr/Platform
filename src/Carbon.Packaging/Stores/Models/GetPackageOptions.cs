namespace Carbon.Packaging
{
    public struct GetPackageOptions
    {
        public bool StripeFirstLevel { get; set; }

        public byte[] EncryptionKey { get; set; }
    }
}