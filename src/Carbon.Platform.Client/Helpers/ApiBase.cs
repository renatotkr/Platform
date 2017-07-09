using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Carbon.Json;
using Carbon.Security.Tokens;

namespace Carbon.Platform
{
    public abstract class ApiBase
    {
        private readonly string baseUri;
        private readonly ISecurityToken credential;

         private readonly HttpClient http = new HttpClient {
            DefaultRequestHeaders = {
                { "User-Agent", "Carbon/1.0.0" },
                { "Accept",     "application/json" }
            },
            Timeout = TimeSpan.FromSeconds(15)
        };

        public ApiBase(Uri endpoint, ISecurityToken credential)
        {
            #region Preconditions

            if (endpoint == null)
                throw new ArgumentNullException(nameof(endpoint));

            if (endpoint.Scheme != "https")
                throw new ArgumentException("scheme must be https", nameof(endpoint));

            #endregion

            this.baseUri    = endpoint.ToString().TrimEnd('/');
            this.credential = credential ?? throw new ArgumentNullException(nameof(credential));
        }

        internal async Task<T> PostAsync<T>(string path, object data)
            where T : new()
        {
            var jsonText = JsonObject.FromObject(data).ToString(pretty: false);

            var request = new HttpRequestMessage(HttpMethod.Post, baseUri + path) {
                Content = new StringContent(jsonText, Encoding.UTF8, "application/json")
            };

            Sign(request);

            using (var response = await http.SendAsync(request).ConfigureAwait(false))
            {
                var text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(text);
                }

                return JsonObject.Parse(text).As<T>();
           }
        }

        internal async Task<MemoryStream> DownloadAsync(string path)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseUri + path);

            Sign(request);

            var ms = new MemoryStream();

            using (var response = await http.SendAsync(request).ConfigureAwait(false))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception("not found");
                }
                
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                }

                var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                await stream.CopyToAsync(ms).ConfigureAwait(false);    
            }

            ms.Position = 0;

            return ms;
        }

        internal async Task<T> UploadAsync<T>(string path, string contentType, Stream stream)
            where T: new()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, baseUri + path) {
                Content = new StreamContent(stream) {
                    Headers = {
                        ContentType = new MediaTypeHeaderValue(contentType)
                    }
                }
            };

            Sign(request);
            
            using (var response = await http.SendAsync(request).ConfigureAwait(false))
            {
                var text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode + ":" + text);
                }

                return JsonObject.Parse(text).As<T>();
            }
        }

        internal async Task<T1> GetAsync<T1>(string path)
            where T1 : new()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseUri + path);

            Sign(request);

            using (var response = await http.SendAsync(request).ConfigureAwait(false))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return default(T1);
                }

                var text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(text);
                }

                try
                {
                    return JsonObject.Parse(text).As<T1>();
                }
                catch
                {
                    throw new Exception("Error parsing:" + text);
                }
            }
        }

        private void Sign(HttpRequestMessage request)
        {
            request.Headers.Date = DateTimeOffset.UtcNow;

            request.Headers.Add("Authorization", "Bearer " + credential.Value);
        }
    }
}