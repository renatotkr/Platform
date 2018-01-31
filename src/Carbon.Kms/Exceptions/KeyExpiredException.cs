using System;

namespace Carbon.Kms
{
    public class KeyExpiredException : Exception
    {
        public KeyExpiredException(IKeyInfo key, DateTime expired)
            : base($"borg:key/{key.Id} is expired") { }
    }
}