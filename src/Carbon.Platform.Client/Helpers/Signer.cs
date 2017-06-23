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

            var claims = new JsonObject {
                { ClaimNames.JwtId,      Guid.NewGuid().ToString().Replace("-", "") },
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
                // caller verification parameters?
                // principal verification parameters?

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
        // aws:role/processor-ai
        // borg:host/1
        public string Principal { get; set; }

        // aws:host/i-1234
        public string HostName { get; set; }

        // { url, headers, body }
        public JsonObject VerificationParameters { get; set; }

        // Used to sign the reqest
        public RSA PrivateKey { get; set; }
    }
}