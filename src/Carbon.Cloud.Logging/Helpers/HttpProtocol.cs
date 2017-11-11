﻿using System;

namespace Carbon.Cloud.Logging
{
    [Flags]
    public enum HttpProtocol : byte
    {
        Unknown = 0,
        Http1   = 1,
        Http2   = 2
    }

    public static class HttpProtocolHelper
    {
        public static HttpProtocol Parse(string text)
        {
            switch (text)
            {
                case "HTTP/1.0":  return HttpProtocol.Unknown;
                case "HTTP/1.1" : return HttpProtocol.Http1;
                case "HTTP/2.0" : return HttpProtocol.Http2;
                case "H2"       : return HttpProtocol.Http2;
                case "h2"       : return HttpProtocol.Http2;
                default         : return HttpProtocol.Unknown;
            }
        }
    }
}