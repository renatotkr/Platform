using System;

namespace Carbon.Kms
{
    public class KeyNotFoundException : Exception
    {
        public KeyNotFoundException(string keyId)
            : base($"borg:key/{keyId} not found") { }
    }
}
