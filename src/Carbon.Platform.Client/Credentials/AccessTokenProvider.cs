using System;
using System.Threading;
using System.Threading.Tasks;

using Carbon.Security.Tokens;

namespace Carbon.Platform
{
    using Security;

    public class AccessTokenProvider : IAccessTokenProvider
    {
        private readonly ICredential credential;
        private readonly OAuth2Client oauth;

        public AccessTokenProvider(string host, ICredential credential)
            : this(credential, new OAuth2Client(Guid.NewGuid(), host)) { }

        public AccessTokenProvider(ICredential credential, OAuth2Client oauth)
        {
            this.credential = credential ?? throw new ArgumentNullException(nameof(credential));
            this.oauth      = oauth      ?? throw new ArgumentNullException(nameof(oauth));
        }

        private readonly SemaphoreSlim gate = new SemaphoreSlim(1);

        public Action OnRenewing { get; set; }

        public async Task RenewAccessTokenAsync()
        {
            OnRenewing?.Invoke();

            var result = await oauth.GetAccessTokenAsync(credential);

            current = new SecurityToken(
                type    : "Bearer", 
                value   : result.AccessToken, 
                expires : DateTime.UtcNow + TimeSpan.FromSeconds(result.ExpiresIn)
            );
        }

        private SecurityToken current = null;

        public SecurityToken Current => current;

        public async ValueTask<SecurityToken> RenewAsync()
        {
            if (current.ShouldRenew())
            {
                await gate.WaitAsync();

                try
                {
                    if (current.ShouldRenew())
                    {
                        await RenewAccessTokenAsync();
                    }
                }
                finally
                {
                    gate.Release();
                }
            }

            return current;            
        }
    }
}