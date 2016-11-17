using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Carbon.Json;

namespace Bitbucket
{
    public class BitbucketClient : IDisposable
    {
        const string baseUri = "https://bitbucket.org/api/2.0";

        private readonly HttpClient httpClient = new HttpClient();

        private readonly NetworkCredential credentials;

        public BitbucketClient(NetworkCredential credentials)
        {
            this.credentials = credentials;
        }

        public async Task<BitbucketCommit> GetCommit(string accountName, string repoName, string revision)
        {
            // https://bitbucket.org/api/2.0

            // POST /repositories/carbonmade/mason/commits/master

            var path = $"/repositories/{accountName}/{repoName}/commits/{revision}";

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, baseUri + path);

            var response = await Send(httpRequest).ConfigureAwait(false);

            return response["values"][0].As<BitbucketCommit>();
        }

        public async Task<MemoryStream> GetZipStream(string accountName, string repoName, string revision)
        {
            var url = $"https://bitbucket.org/{accountName}/{repoName}/get/{revision}.zip";

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);

            httpRequest.Headers.UserAgent.Add(new ProductInfoHeaderValue("Carbon", "1.0.0"));

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue(
                scheme: "Basic",
                parameter: Convert.ToBase64String(Encoding.ASCII.GetBytes($"{credentials.UserName}:{credentials.Password}"))
            );

            using (var response = await httpClient.SendAsync(httpRequest).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var responseText = response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    throw new Exception(responseText + " : " + httpRequest.RequestUri);
                }
                
                var ms = new MemoryStream();

                using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    await stream.CopyToAsync(ms).ConfigureAwait(false);
                }

                ms.Position = 0;

                return ms;
            }
        }

        private async Task<JsonObject> Send(HttpRequestMessage httpRequest)
        {
            httpRequest.Headers.UserAgent.Add(new ProductInfoHeaderValue("Carbon", "1.1.0"));

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue(
                scheme: "Basic",
                parameter: Convert.ToBase64String(Encoding.ASCII.GetBytes($"{credentials.UserName}:{credentials.Password}"))
            );

            using (var response = await httpClient.SendAsync(httpRequest).ConfigureAwait(false))
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(responseText + " : " + httpRequest.RequestUri.ToString());
                }

                return JsonObject.Parse(responseText);
            }
        }


        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
