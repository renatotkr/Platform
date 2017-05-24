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

        public string ExecReload
        {
            get => Get("ExecReload");
            set => Set("ExecReload", value, 4);
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
            set => Set("RestartSec", value, 5);
        }

        // number of restarts during period
        public int? StartLimitInterval
        {
            get => GetInteger("StartLimitInterval");
            set => Set("StartLimitInterval", value, 6);
        }

        public int? StartLimitBurst
        {
            get => GetInteger("StartLimitBurst");
            set => Set("StartLimitBurst", value, 7);
        }

        #endregion

        public string SyslogIdentifier
        {
            get => Get("SyslogIdentifier");
            set => Set("SyslogIdentifier", value, 8);
        }

        /// <summary>
        /// Time waited before forcefully killing
        /// sets both timeoutstartsec and timeoutstopsec
        /// </summary>
        public int? TimeoutSec
        {
            get => GetInteger("TimeoutSec");
            set => Set("TimeoutSec", value, 9);
        }

        public int? TimeoutStartSec
        {
            get => GetInteger("TimeoutStartSec");
            set => Set("TimeoutStartSec", value, 10);
        }

        public int? TimeoutStopSec
        {
            get => GetInteger("TimeoutStopSec");
            set => Set("TimeoutStopSec", value, 11);
        }

        public string User
        {
            get => Get("User");
            set => Set("User", value, 12);
        }

        public string Group
        {
            get => Get("Group");
            set => Set("Group", value, 13);
        }

        // seperated how?
        public string Environment
        {
            get => Get("Environment");
            set => Set("Environment", value, 14);
        }

        #region Limits

        /// <summary>
        /// Maximium file descritors
        /// </summary>
        public int? LimitNoFile
        {
            get => GetInteger("LimitNOFILE");
            set => Set("LimitNOFILE", value, 15);
        }

        public int? LimitNProc
        {
            get => GetInteger("LimitNPROC");
            set => Set("LimitNPROC", value, 16);
        }

        #endregion

        /// <summary>
        /// If using a private /tmp directory, discarded after stop
        /// </summary>
        public bool? PrivateTmp
        {
            get => GetBoolean("PrivateTmp");
            set => Set("PrivateTmp", value, 16);
        }

        public bool? PrivateDevices
        {
            get => GetBoolean("PrivateDevices");
            set => Set("PrivateDevices", value, 16);
        }

        public bool? ProtectHome
        {
            get => GetBoolean("ProtectHome");
            set => Set("ProtectHome", value, 16);
        }

        // true, false, full, strict

        public string ProtectSystem
        {
            get => Get("ProtectSystem");
            set => Set("ProtectSystem", value, 16);
        }

        // Split on space?
        public string ReadWriteDirectories
        {
            get => Get("ReadWriteDirectories");
            set => Set("ReadWriteDirectories", value, 16);
        }

        #region Capabilities

        public string CapabilityBoundingSet
        {
            get => Get("CapabilityBoundingSet");
            set => Set("CapabilityBoundingSet", value, 20);
        }

        public string AmbientCapabilities
        {
            get => Get("AmbientCapabilities");
            set => Set("AmbientCapabilities", value, 21);
        }

        public bool? NoNewPrivileges
        {
            get => GetBoolean("NoNewPrivileges");
            set => Set("NoNewPrivileges", value, 22);
        }

        #endregion
    }
}