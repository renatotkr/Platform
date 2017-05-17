using System;

using Xunit;

namespace Carbon.Platform.Configuration.Tests
{
    public class ServiceConfigurationTests
    {
        [Fact]
        public void Transform()
        {
            var config = new ServiceConfiguration
            {
                Name             = "accelerator",
                Description      = "Accelerator service",
                WorkingDirectory = "/var/apps/accelerator/latest",
                Executable       = new ProgramExecutable("Accelerator", "-port 80"),
                Environment      = new ProgramEnvironment {
                    { "ASPNETCORE_ENVIRONMENT", "Production" }
                },
                RestartPolicy = new ProgramRestartPolicy(RestartCondition.Always, TimeSpan.FromSeconds(10)),
                User = ProgramUser.WwwData
            };

            Assert.Equal(
@"[Unit]
Description=Accelerator service

[Service]
WorkingDirectory=/var/apps/accelerator/latest
ExecStart=/var/apps/accelerator/latest/Accelerator -port 80
Restart=always
RestartSec=10
SyslogIdentifier=accelerator
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target", config.ToSystemdConfiguration().ToString());
        }
    }
}