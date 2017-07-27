using System;

namespace Carbon.Kms
{
    public class KeyExpiredException : Exception
    {
        public KeyExpiredException(IKeyInfo key, DateTime expired)
            : base($"key#{key.Id} expired on {expired} and may no longer be used.") { }
    }
}
