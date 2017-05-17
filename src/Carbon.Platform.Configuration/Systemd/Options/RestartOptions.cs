using System;
using System.Collections.Generic;
using System.Text;

namespace Carbon.Platform.Configuration.Systemd
{
    public static class RestartOptions
    {
        public const string Always = "always";

        // Clean exit code or signal
        public const string OnSuccess = "on-success";

        // Unclean exit code
        public const string OnFailure = "on-failure";

        // On timeout & failure 
        public const string OnAbnormal = "on-abnormal";

        // Watchdog?
        public const string OnWatchdog = "on-watchdog";
    }
}
