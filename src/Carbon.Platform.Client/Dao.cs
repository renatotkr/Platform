using Carbon.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Carbon.Platform
{
    public partial class PlatformClient
    {
        public class Dao<T>
            where T : new()
        {
            protected readonly PlatformClient platform;
            private readonly string prefix;
             
            public Dao(string prefix, PlatformClient platform)
            {
                this.prefix = prefix;
                this.platform = platform;
            }

            public Task<T> GetAsync(long id) => 
                platform.GetAsync<T>($"/{prefix}/{id}");
        }

        private readonly HttpClient http = new HttpClient
        {
            DefaultRequestHeaders = {
                { "User-Agent", "Carbon/1.1.0" },
                { "Accept", "application/json" }
            },
            Timeout = TimeSpan.FromSeconds(15)
        };

        protected async Task<string> PostAsync(string path, object data)
        {
            var postText = JsonObject.FromObject(data).ToString(pretty: false);

            var request = new HttpRequestMessage(HttpMethod.Post, baseUri + path)
            {
                Content = new StringContent(postText, Encoding.UTF8, "application/json")
            };

            Signer.SignRequest(secret, request);

            using (var response = await http.SendAsync(request).ConfigureAwait(false))
            {
                var text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(text);
                }

                return text;
            }
        }

        protected async Task<T1> GetAsync<T1>(string path)
               where T1 : new()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, baseUri + path);

            Signer.SignRequest(secret, request);

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