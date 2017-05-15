using System;

namespace Carbon.Net
{
    using static ApplicationProtocal;

    public enum ApplicationProtocal
    {
        SSH   = 22,
        HTTP  = 80,
        HTTPS = 433,
        MSSQL = 1433,
        MySQL = 3306
    }

    public static class ApplicationProtocalHelper
    {
        public static string Canonicalize(this ApplicationProtocal protocal)
        {
            return protocal.ToString().ToLower();
        }

        public static ApplicationProtocal Parse(string text)
        {
            switch (text)
            {
                case "http"  : return HTTP;
                case "https" : return HTTPS;
                case "ssh"   : return SSH;
                case "mysql" : return MySQL;
                default      : throw new Exception("Unexpected:" + text);
            }
        }
    }
}