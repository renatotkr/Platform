using System;
using System.Net.Http.Headers;

namespace GitHub
{
    public class GitHubCredentials
    {
        public GitHubCredentials(string token, AuthorizationType type = AuthorizationType.Token)
        {
            #region Preconditions

            if (token == null) throw new ArgumentNullException(nameof(token));

            #endregion

            Token = token;
            Type = type;
        }

        public string Token { get; }

        public AuthorizationType Type { get; }

        // "Authorization: token OAUTH-TOKEN"

        public AuthenticationHeaderValue ToHeader()
            => new AuthenticationHeaderValue("token", Token);
    }

    public enum AuthorizationType
    {
        Token = 1
    }
}
