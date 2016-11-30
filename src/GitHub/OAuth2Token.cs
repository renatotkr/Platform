using System;
using System.Net.Http.Headers;

namespace GitHub
{
    public class OAuth2Token
    {
        public OAuth2Token(string value)
        {
            #region Preconditions

            if (value == null) throw new ArgumentNullException(nameof(value));

            #endregion

            Value = value;
        }

        public string Value { get; }

        // "Authorization: token OAUTH-TOKEN"

        public AuthenticationHeaderValue ToHeader()
            => new AuthenticationHeaderValue("token", Value);
    }
}