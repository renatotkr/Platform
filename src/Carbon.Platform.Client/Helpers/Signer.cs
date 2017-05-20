using System;
using System.Linq;
using System.Net.Http;
using System.Text;

using Carbon.Data.Protection;

namespace Carbon.Platform
{
    public static class Signer
    {
        public static void SignRequest(Secret secret, HttpRequestMessage request)
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