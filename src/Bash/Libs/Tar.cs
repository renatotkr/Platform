using System.Text;

namespace Bash.Commands
{
    // sudo tar -xvf {path} --strip 1 -C {targetDirectory}

    public static class Tar
    {
        public static Command Extract(
            string path, 
            string target = null, 
            TarOptions options = TarOptions.None,
            bool stripFirstLevel = false,
            bool sudo = false)
        {
            var sb = new StringBuilder("tar ");

            sb.Append("-");
            sb.Append("x"); // extract

            if (options.HasFlag(TarOptions.Force))
            {
                sb.Append("f");
            }

            if (options.HasFlag(TarOptions.Verbose))
            {
                sb.Append("v");
            }

            if (stripFirstLevel)
            {
                sb.Append(" --strip 1");
            }

            if (target != null)
            {
                sb.Append(" -C " + target);
            }
            
            return new Command(sb.ToString(), sudo);
        }
    }

    public enum TarOptions
    {
        None    = 0,
        Verbose = 1 << 0, // v
        Force   = 2 << 1  // f
    }
}
