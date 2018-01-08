using System;

namespace Carbon.Platform
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message)
            : base(message) { }
    }
}
