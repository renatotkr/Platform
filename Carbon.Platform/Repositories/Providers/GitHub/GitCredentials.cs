namespace GitHub
{
    using System;
    using System.Net.Http.Headers;

    public class GitCredentials
    {
        public GitCredentials(string token, AuthorizationType type = AuthorizationType.Token)
        {
            #region Preconditions

            if (token == null) throw new ArgumentNullException(nameof(token));

            #endregion

            Token = token;
            Type = type;
        }

        public string Token { get; }

        public AuthorizationType Type { get; }

        public AuthenticationHeaderValue ToHeader()
        {
            // "Authorization: token OAUTH-TOKEN"
            return new AuthenticationHeaderValue("token", Token);
        }
    }

    public enum AuthorizationType
    {
        Token = 1
    }

    // TODO: Username + Password
}
