using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Carbon.Json;
using Carbon.Security.Tokens;

namespace Carbon.Platform
{
    using Security;

    internal class AccessTokenService
    {
        private readonly HttpClient http;
        private readonly string baseUri;

        public AccessTokenService(HttpClient http, string baseUri)
        {
            this.http    = http;
            this.baseUri = baseUri;
        }

        public async Task<ISecurityToken> GetAsync(ICredential credential)
        {
            switch (credential)
            {
                case JwtCredential jwk:
                    return await GetAsync(jwk);
                case AccessKeyCredential accessKey :
                    return await GetAsync(accessKey);
                default:
                    throw new Exception("Unexpected credential type:" + credential.GetType().ToString());
            }
        }

        public async Task<ISecurityToken> GetAsync(JwtCredential jwk)
        {
            var parameters = new Dictionary<string, string> {
                { "grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer" },
                { "assertion",  jwk.Encode().ToString() }
            };

            var message = new HttpRequestMessage(HttpMethod.Post, baseUri + "/tokens") {
                Content = new FormUrlEncodedContent(parameters)
            };

            return await SendAsync(message);
        }

        public async Task<ISecurityToken> GetAsync(AccessKeyCredential a)
        {
            var parameters = new Dictionary<string, string> {
                { "grant_type", "client_credentials" }
            };

            if (a.AccountId != null)
            {
                parameters.Add("accountId", a.AccountId.Value.ToString());
            }

            var message = new HttpRequestMessage(HttpMethod.Post, baseUri + "/tokens")
            {
                Headers = {
                    Authorization = new AuthenticationHeaderValue(
                        scheme    : "Basic",
                        parameter : Convert.ToBase64String(
                            Encoding.ASCII.GetBytes(a.AccessKeyId + ":" + a.AccessKeySecret)
                        )
                    )
                },
                Content = new FormUrlEncodedContent(parameters)
            };

            return await SendAsync(message);
        }

        private async Task<SecurityToken> SendAsync(HttpRequestMessage message)
        {
            using (var response = await http.SendAsync(message))
            {
                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) throw new Exception(responseText);

                var result = JsonObject.Parse(responseText).As<OauthTokenInfo>();

                return new SecurityToken("JWT", result.AccessToken, DateTime.UtcNow.AddSeconds(result.ExpiresIn));
            }
        }

        public class OauthTokenInfo
        {
            [DataMember(Name = "type")]
            public string Type { get; set; }

            [DataMember(Name = "access_token")]
            public string AccessToken { get; set; }

            [DataMember(Name = "expires_in")]
            public int ExpiresIn { get; set; }
        }
    }
}