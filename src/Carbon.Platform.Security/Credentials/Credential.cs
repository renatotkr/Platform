using System;
using System.Security.Cryptography;

using Carbon.Json;

namespace Carbon.Platform.Security
{
    public class Credential
    {
        public Credential() { }

        public Credential(string subject, string issuer, string audience, string keyId, RSA privateKey)
        {
            Subject    = subject    ?? throw new ArgumentNullException(nameof(subject));
            Issuer     = issuer     ?? throw new ArgumentNullException(nameof(issuer));
            Audience   = audience   ?? throw new ArgumentNullException(nameof(audience));
            KeyId      = keyId      ?? throw new ArgumentNullException(nameof(keyId));
            PrivateKey = privateKey ?? throw new ArgumentNullException(nameof(privateKey));
        }

        // aws:role/processor-ai
        public string Subject { get; set; }
        
        // to bootstrapper | to cloud
        // https://a.cloud | provider:host/1
        public string Issuer { get; set; }

        // from bootstrapper | from cloud
        // https://a.cloud   | provider:host/1
        public string Audience { get; set; }

        public string KeyId { get; set; }

        // Used to sign the reqest
        // the host & cloud both have a private & public key
        public RSA PrivateKey { get; set; }

        // used to verify the subject with it's authority
        // { url, headers, body }
        public JsonObject VerificationParameters { get; set; }
    }
}