using System;

namespace Carbon.Platform.Security
{
    public class RefreshToken : ICredential
    {
        public RefreshToken(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Value { get; }
    }
}