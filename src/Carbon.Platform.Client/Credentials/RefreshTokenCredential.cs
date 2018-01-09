using System;

namespace Carbon.Platform.Security
{
    public class RefreshTokenCredential : ICredential
    {
        private readonly string value;

        public RefreshTokenCredential(string value)
        {
            this.value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Value { get; }
    }
}