using System.Threading.Tasks;
using Carbon.Security.Tokens;

namespace Carbon.Platform
{
    public interface IAccessTokenProvider
    {
        SecurityToken Current { get; }

        ValueTask<SecurityToken> RenewAsync();
    }
}