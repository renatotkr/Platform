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
                Name = "accelerator",
                WorkingDirectory = "/var/apps/accelerator/latest",
                Executable = new ServiceExecutable("Accelerator", "-port 80"),
                Description = "Accelerator service",
                Environment = new ProgramEnvironment {
                    { "ASPNETCORE_ENVIRONMENT", "Production" }
                },
                RestartPolicy = new RestartPolicy(RestartCondition.Always, TimeSpan.FromSeconds(10)),
                User = ServiceUser.WwwData
            };




            Assert.Equal(
@"[Unit]
Description=accelerator Service

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