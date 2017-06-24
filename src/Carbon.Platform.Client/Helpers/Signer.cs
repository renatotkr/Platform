using System;
using System.Net.Http;
using System.Security.Cryptography;

using Carbon.Json;
using Carbon.Security.Claims;

namespace Carbon.Platform
{
    public static class Signer
    {
        private static readonly JsonObject header = new JsonObject {
            { "typ", "JWT" },
            { "alg", "RS256" }
        };

        public static void SignRequest(
            HttpRequestMessage request,
            Credentials credentials)
        {
            #region Preconditions

            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));

            #endregion

            var date = DateTimeOffset.UtcNow;

            // HexString.FromBytes(Entropy.GenerateBytes(16));

            string tokenId = Guid.NewGuid().ToString().Replace("-", "");

            var claims = new JsonObject {
                { ClaimNames.JwtId,      tokenId },
                { ClaimNames.Subject,    credentials.Principal }, // e.g. aws:role/processor-ai
                { ClaimNames.IssuedAt,   date.ToUnixTimeSeconds() },
                { ClaimNames.Expiration, date.AddMinutes(5).ToUnixTimeSeconds() }
            };

            if (credentials.HostName != null)
            {
                claims.Add("host", credentials.HostName);
            }

            if (credentials.VerificationParameters != null)
            {
                claims.Add("vp", credentials.VerificationParameters);
            }

            var token = Jwt.Jwt.Sign(
                header,
                claims,
                credentials.PrivateKey
            );

            request.Headers.Date = date;
            request.Headers.Add("User-Agent", "Carbon/1.2.0");
            
            var headerValue = "Bearer " + token.ToString();

            request.Headers.TryAddWithoutValidation("Authorization", headerValue);
        }
    }

    public class Credentials
    {
        // borg:host/1;aws:role/processor-ai
        public string Principal { get; set; }

        // host?
        // hostId? 
        public string HostName { get; set; }

        // { url, headers, body }
        public JsonObject VerificationParameters { get; set; }

        // Used to sign the reqest
        public RSA PrivateKey { get; set; }
    }
}