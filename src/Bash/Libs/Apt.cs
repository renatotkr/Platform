using System;
using System.Text;

namespace Bash.Commands
{
    public static class Apt
    {
        public static Command Update() => Command("update");

        public static Command Upgrade() => Command("upgrade");

        public static Command Check() => Command("check");

        public static Command Clean() => Command("clean");

        public static Command Install(params string[] packageNames)
        {
            // apt install -y awscli nginx libunwind8 libcurl4-openssl-dev

            return Command($"install", string.Join(" ", packageNames), options: AptOptions.Yes);
        }

        public static Command Remove(string packageName)
        {
            return Command("remove", packageName, options: AptOptions.Yes);
        }

        #region Helpers

        private static Command Command(
            string commandName, 
            string args = null, 
            bool sudo = true, 
            AptOptions options = default)
        {
            var sb = new StringBuilder("apt ");

            sb.Append(commandName);

            if (options.HasFlag(AptOptions.Yes))
            {
                sb.Append(" -y");
            }

            if (args != null)
            {
                sb.Append(' ');
                sb.Append(args);
            }

            return new Command(sb.ToString(), sudo);
        }

        #endregion
    }

    [Flags]
    public enum AptOptions
    {
        None          = 0,
        DownloadOnly  = 1 << 0,
        Yes           = 1 << 1,
        FixBroken     = 1 << 2,
        IgnoreMissing = 1 << 3,
        Quiet         = 1 << 4,
        Simulate      = 1 << 5,
        ShowUpgraded  = 1 << 6,
        Verbose       = 1 << 7,
        Build         = 1 << 8
    }

}

// ref: https://help.ubuntu.com/community/AptGet/Howto
// ref: https://linux.die.net/man/8/apt-get