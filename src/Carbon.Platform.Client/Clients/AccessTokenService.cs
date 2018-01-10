using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Carbon.Jose;
using Carbon.Json;
using Carbon.Security.Tokens;

namespace Carbon.Platform
{
    using Security;

    // Replace with OAuthClient?

    public class AccessTokenService
    {
        private readonly HttpClient httpClient;
        private readonly string oauthHost;

        private OpenIdConfiguration directory;

        public AccessTokenService(HttpClient httpClient, string oauthHost)
        {
            this.oauthHost  = oauthHost  ?? throw new ArgumentNullException(nameof(oauthHost));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<ISecurityToken> GetAsync(ICredential credential)
        {
            if (credential == null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            if (directory == null)
            {
                await EnsureDirectoryIsSetAsync();
            }

            switch (credential)
            {
                case JwtCredential jwk                   : return await GetFromJwtAsync(jwk);
                case AccessKeyCredential accessKey       : return await GetFromAccessKeyAsync(accessKey);
                case RefreshTokenCredential refreshToken : return await GetFromRefreshTokenAsync(refreshToken);
            }

            throw new Exception("Invalid credential. Was " + credential.GetType().ToString());
        }

        private async Task<ISecurityToken> GetFromJwtAsync(JwtCredential jwk)
        {
            var parameters = new Dictionary<string, string> {
                { "grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer" },
                { "assertion",  jwk.Encode().ToString() }
            };

            var message = new HttpRequestMessage(HttpMethod.Post, directory.TokenEndpoint) {
                Content = new FormUrlEncodedContent(parameters)
            };

            return await SendAsync(message);
        }

        private async Task<ISecurityToken> GetFromRefreshTokenAsync(RefreshTokenCredential refreshToken)
        {
            var parameters = new Dictionary<string, string> {
                { "grant_type",    "refresh_token" },
                { "refresh_token",  refreshToken.Value }
            };

            var message = new HttpRequestMessage(HttpMethod.Post, directory.TokenEndpoint) {
                Content = new FormUrlEncodedContent(parameters)
            };

            return await SendAsync(message);
        }

        private async Task<ISecurityToken> GetFromAccessKeyAsync(AccessKeyCredential accessKey)
        {
            var parameters = new Dictionary<string, string> {
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

            var message = new HttpRequestMessage(HttpMethod.Post, directory.TokenEndpoint) {
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

        private async Task EnsureDirectoryIsSetAsync()
        {
            if (directory == null)
            {
                directory = JsonObject.Parse(
                    text: await httpClient.GetStringAsync($"https://{oauthHost}/.well-known/openid-configuration")
                ).As<OpenIdConfiguration>();
            }
        }

        private async Task<SecurityToken> SendAsync(HttpRequestMessage request)
        {
            request.Version = new Version(2, 0);

            using (var response = await httpClient.SendAsync(request))
            {
                var responseText = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException(responseText);
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode + ":" + responseText);
                }

                var result = JsonObject.Parse(responseText).As<OAuthTokenDetails>();

                return new SecurityToken("JWT", result.AccessToken, DateTime.UtcNow.AddSeconds(result.ExpiresIn));
            }
        }
    }

    public class OAuthTokenDetails
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "expires_in")]
        public int ExpiresIn { get; set; }
    }
}