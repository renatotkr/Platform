using System;

using Carbon.Security.Tokens;

namespace Carbon.Platform
{
    internal static class ISecurityTokenExtensions
    {
        public static bool ShouldRenew(this SecurityToken token)
        {
            return token == null || token.Expires.Value <= DateTime.UtcNow.AddMinutes(-1);
        }
    }
}