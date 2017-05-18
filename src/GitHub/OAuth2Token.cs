using System;
using System.Net.Http.Headers;

namespace GitHub
{
    public struct OAuth2Token
    {
        public OAuth2Token(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Value { get; }

        // "Authorization: token OAUTH-TOKEN"
        // NOTE: they don't use the standard Bearer name...

        public AuthenticationHeaderValue ToHeader()
        {
            return new AuthenticationHeaderValue("token", Value);
        }
    }
}