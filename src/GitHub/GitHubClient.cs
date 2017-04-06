using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Carbon.Json;

namespace GitHub
{
    public class GitHubClient : IDisposable
    {
        private static readonly ProductInfoHeaderValue userAgent = new ProductInfoHeaderValue("Carbon", "1.2.0");

        private readonly string baseUri = "https://api.github.com";

        private readonly HttpClient httpClient = new HttpClient(new HttpClientHandler {
            AllowAutoRedirect = false
        });

        public static readonly int Version = 3;

        private readonly OAuth2Token authToken;

        public GitHubClient(OAuth2Token authToken)
        {
            this.authToken = authToken;
        }

        public async Task<string> CreateAuthorization(string userName, string password, AuthorizationRequest request)
        {
            var postData = JsonObject.FromObject(request).ToString();

            // POST /authorizations
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, baseUri + "/authorizations") {
                Content = new StringContent(postData, Encoding.UTF8, "application/json")
            };

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue(
                scheme    : "Basic",
                parameter : Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{password}"))
            );

            using (var response = await httpClient.SendAsync(httpRequest).ConfigureAwait(false))
            {
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        public async Task<GitUser> GetUser(string userName)
        {
            #region Preconditions

            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            #endregion

            // GET /users/:username
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{baseUri}/users/{userName}");

            var result = await SendAsync(httpRequest, authorize: false).ConfigureAwait(false);

            return result.As<GitUser>();
        }

        public async Task<GitBranch> GetCommit(string accountName, string repoName, string sha)
        {
            #region Preconditions

            if (accountName == null)
                throw new ArgumentNullException(nameof(accountName));

            if (repoName == null)
                throw new ArgumentNullException(nameof(repoName));

            if (sha == null)
                throw new ArgumentNullException(nameof(sha));

            #endregion

            // GET /repos/:owner/:repo/commits/:sha
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{baseUri}/{accountName}/{repoName}/commits/{sha}"
            );

            var result = await SendAsync(request).ConfigureAwait(false);

            return result.As<GitBranch>();
        }

        
        public async Task<GitRef> GetRef(string accountName, string repoName, string refName)
        {
            #region Preconditions

            if (accountName == null)
                throw new ArgumentNullException(nameof(accountName));

            if (repoName == null)
                throw new ArgumentNullException(nameof(repoName));

            #endregion

            // GET /repos/:owner/:repo/git/refs/heads/skunkworkz/featureA

            refName = refName.Replace("refs/", "");

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{baseUri}/repos/{accountName}/{repoName}/git/refs/{refName}"
            );

            var result = await SendAsync(request).ConfigureAwait(false);

            return result.As<GitRef>();
        }

        public async Task<GitRef[]> GetRefs(string accountName, string repoName)
        {
            // GET /repos/:owner/:repo/git/refs
            var request = new HttpRequestMessage(HttpMethod.Get, 
                $"{baseUri}/repos/{accountName}/{repoName}/git/refs"
            );

            var result = await SendAsync(request).ConfigureAwait(false);

            return result.ToArrayOf<GitRef>();
        }

        public async Task<IList<GitBranch>> GetBranches(string accountName, string repositoryName)
        {
            #region Preconditions

            if (accountName == null)
                throw new ArgumentNullException(nameof(accountName));

            if (repositoryName == null)
                throw new ArgumentNullException(nameof(repositoryName));

            #endregion

            // GET /repos/:owner/:repo/branches
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{baseUri}/repos/{accountName}/{repositoryName}/branches"
            );

            var result = await SendAsync(request).ConfigureAwait(false);

            return result.ToArrayOf<GitBranch>();
        }

        /// <summary>
        /// Note: For private repositories, these links are temporary and expire quickly.
        /// </summary>
        public async Task<Uri> GetArchiveLink(GetArchiveLinkRequest request)
        {
            // GET /repos/:owner/:repo/:archive_format/:ref
            // https://api.github.com/repos/user/repo/zipball/dev

            var requestUri = baseUri + request.ToPath();
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);

            httpRequest.Headers.Authorization = authToken.ToHeader();
            httpRequest.Headers.UserAgent.Add(userAgent);

            using (var response = await httpClient.SendAsync(httpRequest).ConfigureAwait(false))
            {
                if (response.StatusCode != System.Net.HttpStatusCode.Redirect)
                {
                    var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    throw new Exception(requestUri + " : " + responseText);
                }

                // Returns a 302 redirect
                var location = response.Headers.GetValues("Location").First();

                return new Uri(location);
            }
        }

        #region Helpers

        private async Task<JsonNode> SendAsync(HttpRequestMessage httpRequest, bool authorize = true)
        {
            if (authorize)
            {
                httpRequest.Headers.Authorization = authToken.ToHeader();
            }

            httpRequest.Headers.UserAgent.Add(userAgent);

            using (var response = await httpClient.SendAsync(httpRequest).ConfigureAwait(false))
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(responseText + " : " + httpRequest.RequestUri);
                }

                return JsonNode.Parse(responseText);
            }
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }

        #endregion
    }
}