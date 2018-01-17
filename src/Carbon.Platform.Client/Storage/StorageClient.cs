using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Carbon.Json;
using Carbon.Storage;

namespace Carbon.Platform.Storage
{
    public class StorageClient
    {
        private readonly HttpClient httpClient = new HttpClient {
            DefaultRequestHeaders = {
                { "User-Agent", "Carbon/1.0" }
            }
        };
        
        private readonly string baseUrl;
        private readonly IAccessTokenProvider accessTokenProvider;

        public StorageClient(string host, IAccessTokenProvider accessTokenProvider)
        {
            this.baseUrl = $"https://{host}/";
            this.accessTokenProvider = accessTokenProvider ?? throw new ArgumentNullException(nameof(accessTokenProvider));
        }
       
        public async Task<IBlobResult> GetObjectAsync(GetObjectRequest request)
        {
            Validate.NotNull(request, nameof(request));

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, baseUrl + request.BucketName + "/" + request.Key);

            await SignAsync(httpRequest);

            return new HttpBlob(request.Key, await httpClient.SendAsync(httpRequest));
        }

        public async Task<IReadOnlyDictionary<string, string>> GetObjectPropertiesAsync(GetObjectPropertiesRequest request)
        {
            Validate.NotNull(request, nameof(request));

            var httpRequest = new HttpRequestMessage(HttpMethod.Head, baseUrl + request.BucketName + "/" + request.Key);

            await SignAsync(httpRequest);
            
            using (var response = await httpClient.SendAsync(httpRequest))
            {
                var httpBlob = new HttpBlob(request.Key, response);
                
                return httpBlob.Properties;                
            }
        }

        public async Task<ObjectDetails> PutObjectAsync(PutObjectRequest request)
        {
            Validate.NotNull(request, nameof(request));

            using (var stream = request.Stream)
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Put, baseUrl + request.BucketName + "/" + request.Key) {
                    Content = new StreamContent(stream)
                };

                SetHeaders(httpRequest, request.Properties);

                var (_, responseText) =  await SendAsync(httpRequest);

                return JsonObject.Parse(responseText).As<ObjectDetails>();
            }
        }

        public async Task DeleteObjectAsync(DeleteObjectRequest request)
        {
            Validate.NotNull(request, nameof(request));

            var httpRequest = new HttpRequestMessage(
                method      : HttpMethod.Delete, 
                requestUri  : baseUrl + request.BucketName + "/" + request.Key
            );

            await SendAsync(httpRequest);
        }

        public async Task<IReadOnlyList<IBlob>> ListBucketAsync(ListBucketRequest request)
        {
            Validate.NotNull(request, nameof(request));

            var sb = new StringBuilder();

            sb.Append(baseUrl);
            sb.Append(request.BucketName);
            sb.Append("/manifest");

            if (request.Prefix != null)
            {
                sb.Append("?filter=");
                sb.Append(request.Prefix);
                sb.Append("*");
            }

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, sb.ToString());

            var (_, responseText) = await SendAsync(httpRequest);

            var list = new List<IBlob>();

            foreach (var entry in JsonArray.Parse(responseText))
            {
                list.Add(entry.As<ObjectDetails>());
            }

            return list;
        }

        #region Helpers

        private async Task<(HttpStatusCode, string)> SendAsync(HttpRequestMessage request)
        {
            await SignAsync(request);

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

        private async ValueTask<bool> SignAsync(HttpRequestMessage request)
        {
            var accessToken = accessTokenProvider.Current.ShouldRenew()
                ? await accessTokenProvider.RenewAsync()
                : accessTokenProvider.Current;

            request.Version = new Version(2, 0); // h2
            
            request.Headers.Add("Authorization", "Bearer " + accessToken.Value);

            return true;
        }  

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
}