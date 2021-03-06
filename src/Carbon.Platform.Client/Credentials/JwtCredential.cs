﻿using System;
using System.Security.Cryptography;

using Carbon.Jose;
using Carbon.Json;

namespace Carbon.Platform.Security
{
    public class JwtCredential : ICredential
    {
        public JwtCredential() { }

        public JwtCredential(string subject, string issuer, string audience, string keyId, RSA privateKey)
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

        // e.g. aws:role/123
        public string Role { get; set; }

        public string KeyId { get; set; }

        // Used to sign the reqest
        // the host & cloud both have a private & public key
        public RSA PrivateKey { get; set; }

        // used to verify the subject with it's authority
        // { url, headers, body }
        public JsonObject VerificationParameters { get; set; }

        public JwtEncodedToken Encode()
        {
            if (string.IsNullOrEmpty(Subject))
                throw new ArgumentException("Required", nameof(Subject));

            if (string.IsNullOrEmpty(Audience))
                throw new ArgumentException("Required", nameof(Audience));

            if (string.IsNullOrEmpty(Issuer))
                throw new ArgumentException("Required", nameof(Issuer));

            if (string.IsNullOrEmpty(KeyId))
                throw new ArgumentException("Required", nameof(KeyId));

            var date = DateTimeOffset.UtcNow;

            var tokenId = Guid.NewGuid().ToString("N");

            var claims = new JsonObject {
                { ClaimNames.JwtId,      tokenId },
                { ClaimNames.Issuer,     Issuer },
                { ClaimNames.Audience,   Audience },
                { ClaimNames.Subject,    Subject }, // e.g. aws:role/processor-ai
                { ClaimNames.IssuedAt,   date.ToUnixTimeSeconds() },
                { ClaimNames.Expiration, date.AddMinutes(15).ToUnixTimeSeconds() }
            };

            // used to verify the subject claim against a trusted third party (i.e. aws:sts)

            if (VerificationParameters != null)
            {
                claims.Add("vp", VerificationParameters);
            }

            if (Role != null)
            {
                claims.Add("role", Role);
            }
            
            var header = new JsonObject {
                { "typ", "JWT" },
                { "alg", JwtAlgorithms.RS256 },
                { "kid", KeyId }
            };   

            return JwtSigner.Default.Sign(header, claims, PrivateKey);
        }
    }
}