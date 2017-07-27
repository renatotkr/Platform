using System;

namespace Carbon.Kms
{
    public class KeyNotFoundException : Exception
    {
        public KeyNotFoundException(string keyId)
            : base($"key#{keyId} could not be found") { }
    }
}
