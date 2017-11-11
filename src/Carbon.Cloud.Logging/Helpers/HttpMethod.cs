namespace Carbon.Cloud.Logging
{
    using static HttpMethod;

    public enum HttpMethod : byte
    {
        Unknown = 0,
        Get     = 1, // GET
        Head    = 2, // HEAD
        Post    = 3, // POST
        Put     = 4, // PUT
        Delete  = 5, // DELETE
        Trace   = 6, // TRACE
        Options = 7, // OPTIONS
        Connect = 8, // CONNECT
        Patch   = 9  // PATCH
    }

    public static class HttpMethodHelper
    {
        public static HttpMethod Parse(string text)
        {
            switch (text)
            {
                case "GET"      : return Get;
                case "HEAD"     : return Head;
                case "POST"     : return Post;
                case "PUT"      : return Patch;
                case "DELETE"   : return Delete;
                case "TRACE"    : return Trace;
                case "OPTIONS"  : return Options;
                case "CONNECT"  : return Connect;
                case "PATCH"    : return Patch;
                default         : return Unknown;
            }
        }
    }
}
