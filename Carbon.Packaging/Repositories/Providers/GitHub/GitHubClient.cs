using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Carbon.Data;

namespace GitHub
{
    public class GitHubClient
    {
        private static readonly ProductInfoHeaderValue userAgent = new ProductInfoHeaderValue("Carbon", "1.0.0");

        private readonly string baseUri = "https://api.github.com";
        private readonly HttpClient httpClient;

        public const int Version = 3;

        private readonly GitHubCredentials auth;

        public GitHubClient(GitHubCredentials credentials)
        {
            #region Preconditions

            if (credentials == null) throw new ArgumentNullException(nameof(credentials));

            #endregion

            this.auth = credentials;
            
            this.httpClient = new HttpClient(new HttpClientHandler {
                AllowAutoRedirect = false
            });
        }

        public async Task<string> CreateAuthorization(string userName, string password, AuthorizationRequest request)
        {
            var postData = XObject.FromObject(request).ToString();

            // POST /authorizations
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, baseUri + "/authorizations") {
                Content = new StringContent(postData, Encoding.UTF8, "application/json")
            };

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue(
                scheme: "Basic",
                parameter: Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{password}"))
            );

            using (var response = await httpClient.SendAsync(httpRequest).ConfigureAwait(false))
            {
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        public async Task<GitUser> GetUser(string userName)
        {
            // GET /users/:username
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{baseUri}/users/{userName}");

            var result = await Send(httpRequest, authorize: false).ConfigureAwait(false);

            return result.As<GitUser>();
        }

        public async Task<GitBranch> GetCommit(string accountName, string repoName, string sha)
        {
            // GET /repos/:owner/:repo/commits/:sha
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{baseUri}/{accountName}/{repoName}/commits/{sha}"
            );

            var result = await Send(request).ConfigureAwait(false);

            return result.As<GitBranch>();
        }

        
        public async Task<GitRef> GetRef(string accountName, string repoName, string refName)
        {
            #region Preconditions

            if (accountName == null) throw new ArgumentNullException(nameof(accountName));

            if (repoName == null) throw new ArgumentNullException(nameof(repoName));

            #endregion

            // GET /repos/:owner/:repo/git/refs/heads/skunkworkz/featureA

            refName = refName.Replace("refs/", "");

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{baseUri}/repos/{accountName}/{repoName}/git/refs/{refName}"
            );

            var result = await Send(request).ConfigureAwait(false);

            return result.As<GitRef>();
        }

        public async Task<GetRefsResult> GetRefs(string accountName, string repoName)
        {
            // GET /repos/:owner/:repo/git/refs
            var request = new HttpRequestMessage(HttpMethod.Get, 
                $"{baseUri}/repos/{accountName}/{repoName}/git/refs"
            );

            var result = await Send(request).ConfigureAwait(false);

            var refs = new GetRefsResult();
            
            foreach (var item in (XArray)result)
            {
                refs.Add(item.As<GitRef>());
            }

            return refs;
        }

        public async Task<IList<GitBranch>> GetBranches(string accountName, string repoName)
        {
            // GET /repos/:owner/:repo/branches
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{baseUri}/repos/{accountName}/{repoName}/branches"
            );

            var result = await Send(request).ConfigureAwait(false);

            return ((XArray)result).ToArrayOf<GitBranch>();
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

            httpRequest.Headers.Authorization = auth.ToHeader();
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

        private async Task<XNode> Send(HttpRequestMessage httpRequest, bool authorize = true)
        {
            if (authorize)
            {
                httpRequest.Headers.Authorization = auth.ToHeader();
            }

            httpRequest.Headers.UserAgent.Add(userAgent);

            using (var response = await httpClient.SendAsync(httpRequest).ConfigureAwait(false))
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(responseText + " : " + httpRequest.RequestUri);
                }

                return XNode.Parse(responseText);
            }
        }

        #endregion
    }
}