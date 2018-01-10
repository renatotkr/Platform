using System;

namespace Carbon.Platform.Security
{
    public class RefreshTokenCredential : ICredential
    {
        public RefreshTokenCredential(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Value { get; }
    }
}