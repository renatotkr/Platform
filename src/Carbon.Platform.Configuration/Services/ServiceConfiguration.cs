using System.Linq;

namespace Carbon.Platform.Configuration
{
    using Systemd;

    public class ServiceConfiguration
    {
        public ServiceConfiguration() { }

        public ServiceConfiguration(
            string name, 
            string workingDirectory,
            ProgramUser user)
        {
            Name             = name;
            WorkingDirectory = workingDirectory;
            User             = user;
        }

        // e.g. accelerator
        // used as the service name, uername, and sysLogIdentifier value
        // [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        
        // default: /var/apps/{name}/latest

        public string WorkingDirectory { get; set; }

        // Install
        // Stop

        // e.g. Accelerator -port 5000
        // fullpath = {workingDirectory}/{name}
        public ProgramExecutable Start { get; set; }

        public ProgramEnvironment Environment { get; set; }

        // StartOn (Boot, Manual)

        public ProgramRestartPolicy RestartPolicy { get; set; }

        public ProgramUser User { get; set; }

        public UnitConfiguration ToSystemdConfiguration()
        {
            var unit = new UnitSection {
                Description = Description ?? (Name + " Service")
            };

            string env = Environment != null
                ? string.Join(" ", Environment.Variables.Select(pair => pair.Key + "=" + pair.Value))
                : null;

            string execStart = Start.FileName.StartsWith("/")
                ? Start.ToString()
                : WorkingDirectory + "/" + Start.ToString();

            var service = new ServiceSection {
                WorkingDirectory = WorkingDirectory,
                ExecStart        = execStart,
                Environment      = env,
                User             = User.ToString(),
                SyslogIdentifier = Name
            };

            if (RestartPolicy != null)
            {
                switch (RestartPolicy.Condition)
                {
                    case RestartCondition.Always    : service.Restart = RestartOptions.Always;     break;
                    case RestartCondition.OnFailure : service.Restart = RestartOptions.OnFailure;  break;
                    case RestartCondition.OnAbormal : service.Restart = RestartOptions.OnAbnormal; break;
                }

                if (RestartPolicy.Delay != null)
                {
                    service.RestartSec = (int)RestartPolicy.Delay.Value.TotalSeconds;
                }
            }

            return new UnitConfiguration {
                Unit = unit,
                Service = service,
                Install = new InstallSection {
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