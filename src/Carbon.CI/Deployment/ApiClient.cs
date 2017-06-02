using System;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Carbon.Data.Protection;

namespace Carbon.CI
{
    public sealed class ApiClient : IDisposable
    {
        private readonly HttpClient http = new HttpClient {
            Timeout = TimeSpan.FromSeconds(15)
        };
        
        private readonly Secret secret;
        private readonly int port;

        public ApiClient(Secret secret, int port)
        {
            this.secret = secret;
            this.port = port;
        }

        internal async Task<string> SendAsync(IPAddress host, string path)
        {
            #region Preconditions

            if (host == null)
                throw new ArgumentNullException(nameof(host));

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            #endregion

            if (path.StartsWith("/"))
            {
                path = path.Trim('/');
            }

            var url = $"http://{host}:{port}/" + path;

            // Update to POST after Bootstrapper v2 migration is completed

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            Sign(request);

            using (var response = await http.SendAsync(request).ConfigureAwait(false))
            {
                var text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("ERROR:" + url + "|" + Environment.NewLine + text);
                }

                return text;
            }
        }

        private void Sign(HttpRequestMessage message)
        {
            // Require a HMAC signed request

            var dateHeader = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(); 

            var stringToSign = string.Join("/n",
                dateHeader,
                message.RequestUri.AbsolutePath
            );

            var signature = Signature.ComputeHmacSha256(
                key  : secret,
                data : Encoding.UTF8.GetBytes(stringToSign)
            );

            message.Headers.Add("User-Agent", "Carbon/1.2.0");
            message.Headers.Add("X-Date", dateHeader);
            message.Headers.Add("X-Signature", signature.ToBase64String());
        }        

        public void Dispose()
        {
            http.Dispose();
        }
    }
}