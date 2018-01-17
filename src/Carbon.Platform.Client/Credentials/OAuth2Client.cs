using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Carbon.Data.Sequences;
using Carbon.Jose;
using Carbon.Json;

namespace Carbon.Platform.Security
{
    public class OAuth2Client
    {
        private readonly HttpClient httpClient = new HttpClient {
            DefaultRequestHeaders = {
                { "User-Agent", "Carbon/2.0" }
            }
        };

        private readonly string clientId;
        private readonly string host;
        private OpenIdConfiguration configuration;

        public OAuth2Client(Uid clientId, string host)
        {
            Ensure.NotNullOrEmpty(host, nameof(host));

            this.clientId = clientId.ToString();
            this.host     = host;
        }

        // Sends an authorization code
        public Task<AuthorizeResult> AuthorizeAsync(string email)
        {
            var request = new AuthorizeRequest(email: email, clientId: clientId);

            return PostJsonAsync<AuthorizeResult>("https://" + host + "/authorize", request);
        }
     
        public async Task<GetAccessTokenResult> GetAccessTokenAsync(ICredential credential)
        {
            if (credential == null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            if (configuration == null)
            {
                await EnsureDirectoryIsSetAsync();
            }

            switch (credential)
            {
                case JwtCredential jwk          : return await GetFromJwtAsync(jwk);
                case AccessKey accessKey        : return await GetFromAccessKeyAsync(accessKey);
                case RefreshToken refreshToken  : return await GetFromRefreshTokenAsync(refreshToken);
            }

            throw new Exception("Invalid credential. Was " + credential.GetType().ToString());
        }
        
        public async Task<GetAccessTokenResult> GetAccessTokenFromAuthorizationCodeAsync(string authorizationCode)
        {
            if (configuration == null)
            {
                await EnsureDirectoryIsSetAsync();
            }

            var parameters = new Dictionary<string, string> {
                { "client_id",  clientId },
                { "grant_type", "authorization_code" },
                { "code",       authorizationCode }
            };

            var message = new HttpRequestMessage(HttpMethod.Post, configuration.TokenEndpoint) {
                Content = new FormUrlEncodedContent(parameters)
            };

            return await SendAsync(message);
        }


        private async Task<GetAccessTokenResult> GetFromJwtAsync(JwtCredential jwk)
        {
            var parameters = new Dictionary<string, string> {
                { "client_id", clientId },
                { "grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer" },
                { "assertion",  jwk.Encode().ToString() }
            };

            var message = new HttpRequestMessage(HttpMethod.Post, configuration.TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(parameters)
            };

            return await SendAsync(message);
        }

        private async Task<GetAccessTokenResult> GetFromRefreshTokenAsync(RefreshToken refreshToken)
        {
            var parameters = new Dictionary<string, string> {
                { "client_id", clientId },
                { "grant_type",    "refresh_token" },
                { "refresh_token",  refreshToken.Value }
            };

            var message = new HttpRequestMessage(HttpMethod.Post, configuration.TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(parameters)
            };

            return await SendAsync(message);
        }

        private async Task<GetAccessTokenResult> GetFromAccessKeyAsync(AccessKey accessKey)
        {
            var parameters = new Dictionary<string, string> {
                { "client_id", clientId },
                { "grant_type", "client_credentials" }
            };

            if (accessKey.Scope != null)
            {
                parameters.Add("scope", accessKey.Scope);
            }

            if (accessKey.AccountId is long accountId)
            {
                parameters.Add("accountId", accountId.ToString());
            }

            var message = new HttpRequestMessage(HttpMethod.Post, configuration.TokenEndpoint)
            {
                Headers = {
                    Authorization = new AuthenticationHeaderValue(
                        scheme    : "Basic",
                        parameter : Convert.ToBase64String(
                            Encoding.ASCII.GetBytes(accessKey.AccessKeyId + ":" + accessKey.AccessKeySecret)
                        )
                    )
                },
                Content = new FormUrlEncodedContent(parameters)
            };

            return await SendAsync(message);
        }

        private Task<T> PostJsonAsync<T>(string url, object data)
            where T : new()
        {
            var jsonText = JsonObject.FromObject(data).ToString(pretty: false);

            var request = new HttpRequestMessage(HttpMethod.Post, url) {
                Content = new StringContent(jsonText, Encoding.UTF8, "application/json")
            };

            return SendAsync<T>(request);
        }

        private async Task EnsureDirectoryIsSetAsync()
        {
            if (configuration == null)
            {
                configuration = JsonObject.Parse(
                    text: await httpClient.GetStringAsync($"https://{host}/.well-known/openid-configuration")
                ).As<OpenIdConfiguration>();
            }
        }

        private async Task<GetAccessTokenResult> SendAsync(HttpRequestMessage request)
        {
            return await SendAsync<GetAccessTokenResult>(request);            
        }

        private async Task<T> SendAsync<T>(HttpRequestMessage request)
            where T : new()
        {
            request.Version = new Version(2, 0);

            using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return default;
                }

                var text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException(text);
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
                catch (Exception ex)
                {
                    throw new Exception("Error parsing:" + text, ex);
                }
            }
        }
    }
}