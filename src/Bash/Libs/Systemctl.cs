namespace Bash.Commands
{
    public static class Systemctl
    {
        public static bool RequiresSudo = true;

        public static Command Enable(string serviceName)
        {
            return Command("enable", serviceName);
        }

        public static Command Disable(string serviceName)
        {
            return Command("disable", serviceName);
        }

        public static Command Start(string serviceName)
        {
            return Command("start", serviceName);
        }

        public static Command Stop(string serviceName)
        {
            return Command("stop", serviceName);
        }

        public static Command Status(string serviceName)
        {
            return Command("status", serviceName);
        }

        public static Command Restart(string serviceName)
        {
            return Command("restart", serviceName);
        }

        public static Command Reload(string serviceName)
        {
            return Command("reload", serviceName);
        }

        private static Command Command(string commandName, string args)
        {
            return new Command($"systemctl {commandName} {args}", RequiresSudo);
        }
    }
}

// sudo systemctl start $APPNAME.service
