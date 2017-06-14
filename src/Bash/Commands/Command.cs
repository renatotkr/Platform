using System;
using System.Text;

namespace Bash.Commands
{
    public struct Command
    {
        public static Command Empty = new Command(null);

        public Command(string text, bool sudo = false)
        {
            Text = text;
            Sudo = sudo;
        }

        public string Text { get; }

        public bool Sudo { get; }

        public override string ToString()
        {
            if (Sudo)
            {
                return "sudo " + Text;
            }

            return Text;
        }

        public static Command Comment(string text)
        {
            return new Command("# " + text);
        }

        public static Command Exit(int statusCode)
        {
            return new Command("exit " + statusCode);
        }

        public static Command Echo(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            #endregion

            return new Command("echo \"" + text + "\"");
        }

        public static Command Set(string name, string value)
        {
            return new Command(name + "=" + value);
        }

        // Options = recurrive (-R)

        // options: verbose, recursive

        // chown [OPTION]... [OWNER][:[GROUP]] FILE...
        

        public static Command CreateSymbolicLink(
            string target, 
            string link,
            SymbolicLinkOptions options, 
            bool sudo = false)
        {
            var sb = new StringBuilder("ln -s");

            if (options.HasFlag(SymbolicLinkOptions.Force))
            {
                sb.Append("f");
            }

            sb.Append(" ");
            sb.Append(target);
            sb.Append(" ");
            sb.Append(link);

            // -s = symbolic
            return new Command($"ln -sfn {target} {link}", sudo);
        }


        public static Command Chown(string owner, string path, bool recursive = false, bool sudo = false)
        {
            #region Preconditions

            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            #endregion

            // sudo chown -R {user} {path}

            var sb = new StringBuilder("chown ");

            if (recursive)
            {
                sb.Append("-R ");
            }

            sb.Append(owner);
            sb.Append(" ");
            sb.Append(path);

            return new Command(sb.ToString(), sudo);
        }

        public static Command ChangeDirectory(string path)
        {
            #region Preconditions

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            #endregion

            return new Command("cd " + path);
        }

        public static Command Remove(string path, bool recursive = false)
        {
            var sb = new StringBuilder("rm ");

            if (recursive)
            {
                sb.Append("-r ");
            }

            sb.Append(path);

            return new Command(sb.ToString());
        }
    }

    public enum SymbolicLinkOptions
    {
        Force    = 1 << 0,  // -f Force existing destination pathnames to be removed to allow the link.
        Symbolic = 1 << 1, // -s 

    }
}
