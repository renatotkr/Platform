using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Carbon.Json;

namespace Carbon.Platform
{
    public partial class PlatformClient
    {
        public class Dao<T>
            where T : new()
        {
            protected readonly PlatformClient platform;
            private readonly string prefix;
             
            internal Dao(string prefix, PlatformClient platform)
            {
                this.prefix = prefix;
                this.platform = platform;
            }

            public Task<T> GetAsync(long id)
            {
                return platform.GetAsync<T>($"/{prefix}/{id}");
            }
        }

        private readonly HttpClient http = new HttpClient {
            DefaultRequestHeaders = {
                { "User-Agent", "Carbon/1.2.0" },
                { "Accept", "application/json" }
            },

            Timeout = TimeSpan.FromSeconds(10)
        };

        protected async Task<T> PostAsync<T>(string path, object data)
            where T : new()
        {
            var jsonText = JsonObject.FromObject(data).ToString(pretty: false);

            var request = new HttpRequestMessage(HttpMethod.Post, baseUri + path) {
                Content = new StringContent(jsonText, Encoding.UTF8, "application/json")
            };

            Signer.SignRequest(subject, request, privateKey);

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

        protected async Task<T1> GetAsync<T1>(string path)
            where T1 : new()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseUri + path);

            Signer.SignRequest(subject, request, privateKey);

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
    }
}