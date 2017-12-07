using System;
using System.Text;

namespace Bash.Commands
{
    public static class Wget
    {
        public static Command Download(
          string url,
          string destination = null,
          WgetOptions options = WgetOptions.None,
          bool sudo = false)
        {
            return Download(new Uri(url), destination, options, sudo);
        }

        public static Command Download(
            Uri url, 
            string destination = null, 
            WgetOptions options = WgetOptions.None,
            bool sudo = false)
        {
            var sb = new StringBuilder("wget ");

            if (options.HasFlag(WgetOptions.Verbose))
            {
                sb.Append("-v ");
            }

            sb.Append('"');
            sb.Append(url.ToString());
            sb.Append('"');

            if (destination != null)
            {
                sb.Append(" -O ");
                sb.Append(destination);
            }

            return new Command(sb.ToString(), sudo);            
        }
    }
    
    [Flags]
    public enum WgetOptions
    {
        None      = 1,
        Verbose   = 1 << 0,
        Quiet     = 1 << 1,
        NoCache   = 1 << 2,
        NoCookies = 1 << 3
    }
}

// wget -v "url" -O /etc/nginx/sites-available/default

// https://www.gnu.org/software/wget/manual/wget.html
