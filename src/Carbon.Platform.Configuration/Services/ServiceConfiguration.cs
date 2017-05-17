using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Carbon.Platform.Configuration
{
    using Systemd;

    public class ServiceConfiguration
    {
        // e.g. accelerator
        // used as the service name, uername, and sysLogIdentifier value
        public string Name { get; set; }

        public string Description { get; set; }
        
        // default: /var/apps/{name}/latest

        public string WorkingDirectory { get; set; }

        // e.g. Accelerator -port 5000
        // fullpath = {workingDirectory}/{name}
        public ServiceExecutable Executable { get; set; }
        
        public ProgramEnvironment Environment { get; set; }

        public ServiceUser User { get; set; }

        public RestartPolicy RestartPolicy { get; set; }

        public UnitConfiguration ToSystemdConfiguration()
        {
            var unit = new UnitSection
            {
                Description = Name + " Service"
            };

            int? restartSec = null;

            if (RestartPolicy?.Delay != null)
            {
                restartSec = (int)RestartPolicy.Delay.Value.TotalSeconds;
            }

            string env = Environment != null
                ? string.Join(" ", Environment.Variables.Select(pair => pair.Key + "=" + pair.Value))
                : null;

            var service = new ServiceSection
            {
                WorkingDirectory = WorkingDirectory,
                ExecStart        = WorkingDirectory + "/" + Executable.ToString(),
                Restart          = RestartPolicy?.Condition == RestartCondition.Always ? "always" : null,
                Environment      = env,
                RestartSec       = restartSec,
                User             = User.ToString(),
                SyslogIdentifier = Name
            };

            return new UnitConfiguration
            {
                Unit = unit,
                Service = service,
                Install = new InstallSection
                {
                    WantedBy = "multi-user.target"
                }
            };
        }
    }
}

/*
A depedency can be a package or runtime
// e.g. awscli nginx libunwind8 libcurl4-openssl-dev

public PackageInfo[] Dependencies { get; set; }
        
public string[] InstallCommands { get; set; }

public string[] UninstallCommands { get; set; }

public string[] UpdateCommands { get; set; }
*/