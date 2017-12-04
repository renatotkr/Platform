using System;
using System.Text;

namespace Bash.Commands
{
    // sudo tar -xvf {path} --strip 1 -C {targetDirectory}

    public static class Tar
    {
        public static Command Extract(
            string file,
            string directory = null,
            TarOptions options = TarOptions.None,
            bool stripFirstLevel = false,
            bool sudo = false)
        {
            #region Preconditions

            if (file == null)
                throw new ArgumentNullException(nameof(file));

            #endregion

            options |= TarOptions.Extract | TarOptions.File;

            if (file.EndsWith(".gz"))
            {
                options |= TarOptions.Gzip;
            }

            var sb = new StringBuilder("tar ");

            sb.Append(GetOptionText(options));

            sb.Append(' ');

            sb.Append(file);

            if (stripFirstLevel)
            {
                sb.Append(" --strip 1");
            }

            if (directory != null)
            {
                sb.Append(" -C ");
                sb.Append(directory);
            }
            
            return new Command(sb.ToString(), sudo);
        }

        // http://www.tutorialspoint.com/unix_commands/tar.htm
        public static string GetOptionText(TarOptions options)
        {
            var sb = new StringBuilder("-");

            if (options.HasFlag(TarOptions.Extract))
            {
                sb.Append('x');
            }

            if (options.HasFlag(TarOptions.Gzip))
            {
                sb.Append('z');
            }

            if (options.HasFlag(TarOptions.Create))
            {
                sb.Append('c');
            }

            if (options.HasFlag(TarOptions.File))
            {
                sb.Append('f');
            }

            if (options.HasFlag(TarOptions.Verbose))
            {
                sb.Append('v');
            }

            if (options.HasFlag(TarOptions.PerservePermissions))
            {
                sb.Append('p');
            }

            return sb.ToString();
        }
    }

    [Flags]
    public enum TarOptions
    {
        None                = 0,
        Verbose             = 1 << 0, // v
        File                = 1 << 1, // f
        PerservePermissions = 1 << 2, // p
        Gzip                = 1 << 3, // z
        Extract             = 1 << 4, // x
        Create              = 1 << 5  // c
    }
}

// ref: http://www.tutorialspoint.com/unix_commands/tar.htm