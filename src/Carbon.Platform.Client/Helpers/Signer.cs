using System;
using System.Net.Http;
using System.Security.Cryptography;

using Carbon.Json;
using Carbon.Security.Claims;

namespace Carbon.Platform
{
    public static class Signer
    {
        public static void SignRequest(string subject, HttpRequestMessage request, RSA privateKey)
        {
            #region Preconditions

            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));

            #endregion

            var token = Carbon.Jwt.Jwt.Sign(
                new JsonObject {
                    { "typ", "JWT" },
                    { "alg", "RS256" }
                },
                new JsonObject {
                    { ClaimNames.JwtId,      Guid.NewGuid().ToString().Replace("-", "") },
                    { ClaimNames.Subject,    subject }, // e.g. host#4502452345
                    { ClaimNames.Expiration, DateTimeOffset.UtcNow.AddMinutes(5).ToUnixTimeSeconds() }
                },
                privateKey
            );

            var now = DateTimeOffset.UtcNow;

            request.Headers.Date = now;
            request.Headers.Add("User-Agent", "Carbon/1.2.0");
            
            var headerValue = "Bearer " + token.ToString();

            request.Headers.TryAddWithoutValidation("Authorization", headerValue);
        }
    }
}