using System;
using System.Threading.Tasks;

using Carbon.Jwt;
using Carbon.OAuth2;

namespace Carbon.Platform.Security
{
    public class AuthenticationService : OAuth2Client
    {
        public AuthenticationService(string clientId, string clientSecret, OAuth2Endpoints endpoints)
            : base(clientId, clientSecret, endpoints) { }

        public Task<IAccessToken> GetAccessTokenAsync(JwtCredential credential, string scope = null)
        {
            #region Preconditions

            if (credential == null)
                throw new ArgumentNullException(nameof(credential));

            #endregion

            return GetAccessTokenAsync(credential.Encode(), scope);
        }

        public async Task<IAccessToken> GetAccessTokenAsync(EncodedJwtToken token, string scope = null)
        {
            var request = AccessTokenRequest.FromJwt(
                jwtEncodedToken : token.ToString(),
                scope           : scope
            );

            var result = await GetAccessTokenAsync(request).ConfigureAwait(false);

            return result.ToAccessToken();
        }
    }
}