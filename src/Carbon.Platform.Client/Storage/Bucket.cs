using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Carbon.Json;
using Carbon.Platform.Security;
using Carbon.Security.Tokens;
using Carbon.Storage;

namespace Carbon.Platform.Storage
{
    public class Bucket : IBucket
    {
        public static readonly HttpClient httpClient = new HttpClient {
            DefaultRequestHeaders = {
                { "User-Agent", "Carbon/1.0" }
            }
        };
        
        private readonly string name;
        private readonly string baseUrl;
        private readonly ICredential credential;
        private readonly AccessTokenService accessTokenService;

        public Bucket(string host, string name, ICredential credential, string oauthHost)
        {
            this.name       = name ?? throw new ArgumentNullException(nameof(name));
            this.credential = credential;

            this.baseUrl = $"https://{host}/{name}/";

            // Inject?

            this.accessTokenService = new AccessTokenService(httpClient, oauthHost);
        }

        public async Task<IBlob> GetAsync(string key)
        {
            return await GetAsync(key, new GetBlobOptions());
        }

        public async Task<IBlobResult> GetAsync(string key, GetBlobOptions options)
        {
            // TODO: Support blob options

            var request = new HttpRequestMessage(HttpMethod.Get, baseUrl + key) {
                Version = new Version(2, 0)
            };

            await SignAsync(request);

            return new HttpBlob(key, await httpClient.SendAsync(request));
        }

        public async Task<IReadOnlyDictionary<string, string>> GetPropertiesAsync(string key)
        {
            var request = new HttpRequestMessage(HttpMethod.Head, baseUrl + key) {
                Version = new Version(2, 0)
            };

            await SignAsync(request);

            var properties = new BlobProperties();
            
            using (var response = await httpClient.SendAsync(request))
            {
                foreach (var header in response.Headers)
                {
                    properties[header.Key] = string.Join(";", header.Value);
                }
            }

            return properties;
        }

        public Task PutAsync(IBlob blob)
        {
            return PutAsync(blob, null);
        }

        public async Task PutAsync(IBlob blob, PutBlobOptions options)
        {
            if (options?.EncryptionKey != null)
            {
                throw new Exception("Encryption keys not yet supported");
            }

            using (var stream = await blob.OpenAsync())
            {
                var request = new HttpRequestMessage(HttpMethod.Put, baseUrl + blob.Key) {
                    Content = new StreamContent(stream)
                };

                SetHeaders(request, blob.Properties);

                await SendAsync(request);
            }
        }

        public async Task DeleteAsync(string key)
        {
            await SendAsync(new HttpRequestMessage(HttpMethod.Delete, baseUrl + key));
        }

        public async Task<IReadOnlyList<IBlob>> ListAsync(string prefix = null)
        {
            var url = baseUrl + "manifest";

            if (prefix != null)
            {
                url += "?filter=" + prefix + "*";
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            
            var (_, responseText) = await SendAsync(request);

            var list = new List<IBlob>();

            foreach (var entry in JsonArray.Parse(responseText))
            {
                list.Add(entry.As<BlobDetails>());
            }

            return list;
        }

        #region Helpers

        private async Task<(HttpStatusCode, string)> SendAsync(HttpRequestMessage request)
        {
            await SignAsync(request);

            request.Version = new Version(2, 0);

            using (var response = await httpClient.SendAsync(request))
            {
                var responseText = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException(responseText);
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode + ":" + responseText);
                }

                return (response.StatusCode, responseText);
            }
        }

        private ISecurityToken accessToken;
        private readonly SemaphoreSlim gate = new SemaphoreSlim(1);

        private async ValueTask<bool> SignAsync(HttpRequestMessage request)
        {
            if (accessToken.ShouldRenew())
            {
                await gate.WaitAsync();

                try
                {
                    if (accessToken.ShouldRenew())
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

            return true;
        }

        private async Task<ISecurityToken> RenewAccessToken()
        {
            accessToken = await accessTokenService.GetAsync(credential);

            return accessToken;
        }

        #endregion

        #region Helpers

        private void SetHeaders(HttpRequestMessage request, IReadOnlyDictionary<string, string> headers)
        {
            if (headers == null) return;

            foreach (var item in headers)
            {
                switch (item.Key)
                {
                    case "Content-Encoding":
                        request.Content.Headers.ContentEncoding.Add(item.Value); break;
                    case "Content-Type":
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue(item.Value); break;

                    // Skip list...
                    case "Accept-Ranges":
                    case "Content-Length":
                    case "Date":
                    case "ETag":
                    case "Server":
                    case "Last-Modified":
                        break;

                    default:
                        request.Headers.Add(item.Key, item.Value); break;
                }
            }
        }

        #endregion
    }

    internal class BlobDetails : IBlob
    {
        public string Key { get; set; }

        public long Size { get; set; }

        public DateTime Modified { get; set; }

        public IReadOnlyDictionary<string, string> Properties { get; set; }

        public void Dispose() { }

        public ValueTask<Stream> OpenAsync()
        {
            throw new NotImplementedException();
        }
    }
}