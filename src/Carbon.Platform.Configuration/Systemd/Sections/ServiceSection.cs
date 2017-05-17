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

        public string ExecStop
        {
            get => Get("ExecStop");
            set => Set("ExecStop", value, 3);
        }

        #region Restart Options

        public string Restart
        {
            get => Get("Restart");
            set => Set("Restart", value, 4);
        }

        // The time to delay before restarting the service
        // defaults to 100ms
        public int? RestartSec
        {
            get => GetInteger("RestartSec");
            set => SetInteger("RestartSec", value, 5);
        }

        #endregion

        public string SyslogIdentifier
        {
            get => Get("SyslogIdentifier");
            set => Set("SyslogIdentifier", value, 6);
        }

        /// <summary>
        /// Time waited before forcefully killing
        /// sets both timeoutstartsec and timeoutstopsec
        /// </summary>
        public int? TimeoutSec
        {
            get => GetInteger("TimeoutSec");
            set => SetInteger("TimeoutSec", value, 7);
        }

        public int? TimeoutStartSec
        {
            get => GetInteger("TimeoutStartSec");
            set => SetInteger("TimeoutStartSec", value, 8);
        }

        public int? TimeoutStopSec
        {
            get => GetInteger("TimeoutStopSec");
            set => SetInteger("TimeoutStopSec", value, 9);
        }

        public string User
        {
            get => Get("User");
            set => Set("User", value, 10);
        }

        // seperated how?
        public string Environment
        {
            get => Get("Environment");
            set => Set("Environment", value, 11);
        }
    }
}