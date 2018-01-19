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
        private readonly HttpClient httpClient = new HttpClient(new HttpClientHandler {
            AllowAutoRedirect = false
        }) {
            DefaultRequestHeaders = {
                { "User-Agent", "Carbon/2.0" }
            }
        };

        public static readonly int Version = 3;

        private readonly OAuth2Token accessToken;

        public GitHubClient(OAuth2Token accessToken)
        {
            this.accessToken = accessToken;
        }

        #region Authorizations

        public async Task<Authorization> CreateAuthorizationAsync(
            string userName,
            string password,
            AuthorizationRequest request)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("Required", nameof(userName));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Required", nameof(password));

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
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                try
                {
                    return JsonObject.Parse(responseText).As<Authorization>();
                }
                catch
                {
                    throw new Exception(responseText);
                }
            }
        }

        #endregion

        public async Task<GitUser> GetUserAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Required", nameof(userName));
            }

            // GET /users/:username
            var result = await SendAsync(
                method    : HttpMethod.Get,
                path      : $"/users/{userName}",
                authorize : false
            ).ConfigureAwait(false);

            return result.As<GitUser>();
        }

        #region Commits

        // https://developer.github.com/v3/git/commits/

        public async Task<GitCommit> GetCommitAsync(string accountName, string repoName, string sha)
        {
            #region Preconditions

            if (accountName == null)
                throw new ArgumentNullException(nameof(accountName));

            if (repoName == null)
                throw new ArgumentNullException(nameof(repoName));

            if (sha == null)
                throw new ArgumentNullException(nameof(sha));

            #endregion

            // GET /repos/:owner/:repo/git/commits/:sha
            var result = await SendAsync(
                method : HttpMethod.Get,
                path   : $"/repos/{accountName}/{repoName}/git/commits/{sha}"
            ).ConfigureAwait(false);

            return result.As<GitCommit>();
        }

        #endregion

        #region Refs

        public async Task<GitRef> GetRefAsync(string accountName, string repoName, string refName)
        {
            #region Preconditions

            if (accountName == null)
                throw new ArgumentNullException(nameof(accountName));

            if (repoName == null)
                throw new ArgumentNullException(nameof(repoName));

            #endregion

            // GET /repos/:owner/:repo/git/refs/heads/skunkworkz/featureA

            refName = refName.Replace("refs/", string.Empty);

            var result = await SendAsync(
                method : HttpMethod.Get,
                path   : $"/repos/{accountName}/{repoName}/git/refs/{refName}"
            ).ConfigureAwait(false);

            return result.As<GitRef>();
        }

        public async Task<GitRef[]> GetRefsAsync(string accountName, string repoName)
        {
            // GET /repos/:owner/:repo/git/refs
            
            var result = await SendAsync(
                method : HttpMethod.Get,
                path    : $"/repos/{accountName}/{repoName}/git/refs"
            ).ConfigureAwait(false);

            return result.ToArrayOf<GitRef>();
        }

        #endregion

        #region Repositories

        public async Task<GitRepository> GetRepositoryAsync(string accountName, string repositoryName)
        {
            #region Preconditions

            if (accountName == null)
                throw new ArgumentNullException(nameof(accountName));

            if (repositoryName == null)
                throw new ArgumentNullException(nameof(repositoryName));

            #endregion

            // GET /repos/:owner/:repo

            var result = await SendAsync(
                method : HttpMethod.Get,
                path   : $"/repos/{accountName}/{repositoryName}"
            ).ConfigureAwait(false);

            return result.As<GitRepository>();
        }

        public async Task<GitCommit[]> GetRepositoryCommitsAsync(string accountName, string repositoryName)
        {
            #region Preconditions

            if (accountName == null)
                throw new ArgumentNullException(nameof(accountName));

            if (repositoryName == null)
                throw new ArgumentNullException(nameof(repositoryName));

            #endregion

            // GET /repos/:owner/:repo/commits

            var result = await SendAsync(
                method: HttpMethod.Get,
                path: $"/repos/{accountName}/{repositoryName}/commits"
            ).ConfigureAwait(false);

            return result.ToArrayOf<GitCommit>();
        }

        #endregion

        #region Branches

        public async Task<IList<GitBranch>> GetBranchesAsync(string accountName, string repositoryName)
        {
            #region Preconditions

            if (accountName == null)
                throw new ArgumentNullException(nameof(accountName));

            if (repositoryName == null)
                throw new ArgumentNullException(nameof(repositoryName));

            #endregion

            // GET /repos/:owner/:repo/branches
            
            var result = await SendAsync(
                method : HttpMethod.Get,
                path   : $"/repos/{accountName}/{repositoryName}/branches"
            ).ConfigureAwait(false);

            return result.ToArrayOf<GitBranch>();
        }

        #endregion
        
        #region Archives

        /// <summary>
        /// Note: For private repositories, these links are temporary and expire quickly.
        /// </summary>
        public async Task<Uri> GetArchiveLinkAsync(GetArchiveLinkRequest request)
        {
            // GET /repos/:owner/:repo/:archive_format/:ref
            // https://api.github.com/repos/user/repo/zipball/dev

            var httpRequest = new HttpRequestMessage(
                method     : HttpMethod.Get,
                requestUri : baseUri + request.ToPath()
            );

            httpRequest.Headers.Authorization = accessToken.ToHeader();

            using (var response = await httpClient.SendAsync(httpRequest).ConfigureAwait(false))
            {
                if (response.StatusCode != System.Net.HttpStatusCode.Redirect)
                {
                    var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    throw new Exception(httpRequest.RequestUri + " : " + responseText);
                }

                // Returns a 302 redirect
                var location = response.Headers.GetValues("Location").First();

                return new Uri(location);
            }
        }

        #endregion

        #region Helpers

        private const string baseUri = "https://api.github.com";

        private async Task<JsonNode> SendAsync(
            HttpMethod method, 
            string path,
            bool authorize = true)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri: baseUri + path) {
                Headers = {
                    { "Accept", "application/vnd.github.v3+json" }
                }
            };

            if (authorize)
            {
                request.Headers.Authorization = accessToken.ToHeader();
            }

            using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(responseText + " : " + request.RequestUri);
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

// ref: https://developer.github.com/v3/