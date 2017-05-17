namespace Carbon.Platform.Configuration.Systemd
{
    public class ServiceSection : SectionBase
    {
        public string WorkingDirectory
        {
            get => Get("WorkingDirectory");
            set => Set("WorkingDirectory", value, 1);
        }

        public string ExecStart
        {
            get => Get("ExecStart");
            set => Set("ExecStart", value, 2);
        }

        // always, on-success, on-failure, on-abnormal, on-abort, on-watchdog
        public string Restart
        {
            get => Get("Restart");
            set => Set("Restart", value, 3);
        }

        public int? RestartSec
        {
            get => GetInteger("RestartSec");
            set => SetInteger("RestartSec", value, 4);
        }

        public string SyslogIdentifier
        {
            get => Get("SyslogIdentifier");
            set => Set("SyslogIdentifier", value, 5);
        }

        /// <summary>
        /// Time waited before forcefully killing
        /// </summary>
        public int? TimeoutSec
        {
            get => GetInteger("TimeoutSec");
            set => SetInteger("TimeoutSec", value, 4);
        }

        public string User
        {
            get => Get("User");
            set => Set("User", value, 6);
        }

        // seperated how?
        public string Environment
        {
            get => Get("Environment");
            set => Set("Environment", value, 7);
        }
    }
}