using System;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Carbon.Platform
{
    using Carbon.Security;
    using Protection;

    public static class Signer
    {
        public static void SignRequest(SecretKey secret, HttpRequestMessage request)
        {
            #region Preconditions

            if (secret.Value == null) throw new ArgumentNullException(nameof(secret.Value));

            #endregion

            var now = DateTimeOffset.UtcNow;

            request.Headers.Date = now;
            request.Headers.Add("User-Agent", "Carbon/1.1.0");
            
            var dateHeader = request.Headers.GetValues("Date").First();
            
            var stringToSign = string.Join("\n",
                dateHeader,
                request.Method.ToString().ToUpper(),
                request.RequestUri.Authority,
                request.RequestUri.AbsolutePath
            );

            var signature = Signature.ComputeHmacSha256(
                key  : secret,
                data : Encoding.UTF8.GetBytes(stringToSign)
            );
            
            var headerValue = $"TBD Signature={signature.ToHexString()}";

            request.Headers.TryAddWithoutValidation("Authorization", headerValue);
        }
    }
}
