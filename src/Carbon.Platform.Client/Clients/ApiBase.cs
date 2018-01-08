using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Carbon.Data.Protection;
using Carbon.Json;
using Carbon.Security.Tokens;

namespace Carbon.Platform
{
    using Security;

    public abstract class ApiBase
    {
        private readonly string baseUri;
        private readonly ICredential credential;
        private readonly AccessTokenService accessTokenService;
        
        private readonly HttpClient http = new HttpClient {
            DefaultRequestHeaders = {
                { "User-Agent", "Carbon/1.0" },
                { "Accept",     "application/json" }
            },
            Timeout = TimeSpan.FromSeconds(15)
        };

        public ApiBase(Uri endpoint, ICredential credential)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            if (endpoint.Scheme != "https")
            {
                throw new ArgumentException("scheme must be https", nameof(endpoint));
            }

            this.baseUri            = endpoint.ToString().TrimEnd('/');
            this.credential         = credential ?? throw new ArgumentNullException(nameof(credential));
            this.accessTokenService = new AccessTokenService(http, baseUri);
        }

        internal async Task<MemoryStream> DownloadAsync(string path)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseUri + path) {
                Version = new Version(2, 0)
            };

            await SignRequestAsync(request);

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

        internal Task<T> PutStreamAsync<T>(
            string path, 
            string contentType, 
            Stream stream, 
            JsonObject properties)
            where T: new()
        {
            var sha256 = Hash.ComputeSHA256(stream, true);

            stream.Position = 0;

            var request = new HttpRequestMessage(HttpMethod.Put, baseUri + path) {
                Version = new Version(2, 0),
                Content = new StreamContent(stream) {
                    Headers = {
                        ContentType = new MediaTypeHeaderValue(contentType)
                    }
                },
                Headers = {
                    { "X-Content-SHA256", sha256.ToBase64String() },
                    { "X-Properties",     properties.ToString(pretty: false) }
                }
            };

            return SendAsync<T>(request);
        }

        internal Task<T> PostAsync<T>(string path, object data)
             where T : new()
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var jsonText = JsonObject.FromObject(data).ToString(pretty: false);

            var request = new HttpRequestMessage(HttpMethod.Post, baseUri + path) {
                Version = new Version(2, 0),
                Content = new StringContent(jsonText, Encoding.UTF8, "application/json")
            };

            return SendAsync<T>(request);
        }

        internal Task<T> PatchAsync<T>(string path, object data)
             where T : new()
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var jsonText = JsonObject.FromObject(data).ToString(pretty: false);

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), baseUri + path) {
                Version = new Version(2, 0),
                Content = new StringContent(jsonText, Encoding.UTF8, "application/json")
            };

            return SendAsync<T>(request);
        }

        internal Task<T> GetAsync<T>(string path)
            where T : new()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseUri + path) {
                Version = new Version(2, 0)
            };

            return SendAsync<T>(request);
        }

        internal async Task<T[]> GetListAsync<T>(string path)
            where T : new()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseUri + path) {
                Version = new Version(2, 0)
            };

            await SignRequestAsync(request);

            using (var response = await http.SendAsync(request).ConfigureAwait(false))
            {
                var text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                // TODO: standarize error response and return a message message
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(text);
                }

                try
                {
                    return JsonNode.Parse(text).ToArrayOf<T>();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error parsing:" + text, ex);
                }
            }
        }

        private async Task<T> SendAsync<T>(HttpRequestMessage request)
            where T : new()
        {
            await SignRequestAsync(request);

            using (var response = await http.SendAsync(request).ConfigureAwait(false))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return default;
                }

                var text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);


                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException("unauthorized:" + text);
                }

                // TODO: standarize error response and return a message message
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString() + ":" + text);
                }

                try
                {
                    return JsonNode.Parse(text).As<T>();
                }
                catch(Exception ex)
                {
                    throw new Exception("Error parsing:" + text, ex);
                }
            }
        }
      
        private ISecurityToken accessToken;
        private readonly SemaphoreSlim gate = new SemaphoreSlim(1);

        private async Task SignRequestAsync(HttpRequestMessage request)
        {
            if (ShouldRenewToken(accessToken))
            {
                await gate.WaitAsync();

                try
                {
                    if (ShouldRenewToken(accessToken))
                    {
                        await RenewAccessToken();
                    }
                }
                finally
                {
                    gate.Release();
                }
            }

            request.Headers.Date = DateTimeOffset.UtcNow;

            request.Headers.Add("Authorization", "Bearer " + accessToken.Value);
        }

        public Action AccessTokenRenewing { get; set; }

        public async Task RenewAccessToken()
        {
            AccessTokenRenewing?.Invoke();

            accessToken = await accessTokenService.GetAsync(credential);       
        }

        #region Helpers

        private static bool ShouldRenewToken(ISecurityToken token)
        {
            return token == null || token.Expires.Value <= DateTime.UtcNow.AddMinutes(-1);
        }

        #endregion
    }
}