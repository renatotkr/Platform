using System;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Carbon.Platform
{
    using Protection;
    using Security;

    public static class Signer
    {
        public static void SignRequest(SecretKey secret, HttpRequestMessage request)
        {
            #region Preconditions

            if (secret.Value == null) throw new ArgumentNullException(nameof(secret.Value));

            #endregion

            request.Headers.Date = DateTimeOffset.UtcNow;
            request.Headers.Add("User-Agent", "Carbon/1.0");

            var dateHeader = request.Headers.GetValues("Date").First();
            
            var stringToSign = string.Join("\n",
                "HMAC-SHA256",                       // Algorithm
                dateHeader,                          // Request Date / TODO: Format as ISO
                request.Method.ToString().ToUpper(),
                request.RequestUri.Authority,
                request.RequestUri.AbsolutePath
            );

            var signature = Signature.ComputeHmacSha256(
                key  : secret,
                data : Encoding.UTF8.GetBytes(stringToSign)
            );

            // TODO: Credential

            var headerValue = $"C Algorithm=HMAC-SHA256,Signature={signature.ToHexString()}";

            request.Headers.TryAddWithoutValidation("Authorization", headerValue);
        }
    }
}
