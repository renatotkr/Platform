﻿using System;
using System.Net.Http;

using Carbon.Json;
using Carbon.Jwt;

namespace Carbon.Platform.Security
{
    public static class Signer
    {
        private static readonly JsonObject defaultHeader = new JsonObject {
            { "typ", "JWT" },
            { "alg", "RS256" }
        };

        // Expire in 15 minutes
        public static void SignRequest(HttpRequestMessage request, Credential credential)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (credential == null)
                throw new ArgumentNullException(nameof(credential));

            if (credential.Subject == null)
                throw new ArgumentException("Subject is required", nameof(credential));

            if (credential.Audience == null)
                throw new ArgumentException("Audience is required", nameof(credential));

            if (credential.Issuer == null)
                throw new ArgumentException("Issuer is required", nameof(credential));

            #endregion

            var date = DateTimeOffset.UtcNow;
            
            var tokenId = Guid.NewGuid().ToString("N");

            var claims = new JsonObject {
                { ClaimNames.JwtId,      tokenId },
                { ClaimNames.Issuer,     credential.Issuer },
                { ClaimNames.Audience,   credential.Audience },
                { ClaimNames.Subject,    credential.Subject }, // e.g. aws:role/processor-ai
                { ClaimNames.IssuedAt,   date.ToUnixTimeSeconds() },
                { ClaimNames.Expiration, date.AddMinutes(15).ToUnixTimeSeconds() }
            };

            // used to verify the subject claim against a trusted third party (i.e. aws:sts)

            if (credential.VerificationParameters != null)
            {
                claims.Add("vp", credential.VerificationParameters);
            }

            JsonObject header;

            if (credential.KeyId != null)
            {
                header = new JsonObject {
                    { "typ", "JWT" },
                    { "alg", "RS256" },
                    { "kid",  credential.KeyId }
                };
            }
            else
            {
                header = defaultHeader;
            }

            var token = JwtSigner.Sign(header, claims, credential.PrivateKey);

            request.Headers.Date = date;
            request.Headers.Add("User-Agent", "Carbon/2.0.0");
            
            var headerValue = "Bearer " + token.ToString();

            request.Headers.TryAddWithoutValidation("Authorization", headerValue);
        }
    }
}