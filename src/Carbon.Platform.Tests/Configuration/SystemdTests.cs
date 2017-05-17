using Xunit;

using Carbon.Platform.Configuration.Systemd;

namespace Carbon.Platform.Configuration.Tests
{
    public class SystemdTests
    {
        [Fact]
        public void SystemdTest()
        {
            var unit = new UnitConfiguration
            {
                Unit = new UnitSection {
                    Description = "Accelerator"
                },

                Service = new ServiceSection {
                    User = "www-user",
                    Restart = "always",
                    RestartSec = 10,
                    ExecStart = "/var/apps/Accelerator -port 80",
                    WorkingDirectory = "/var/apps/accelerator/latest"
                },

                Install = new InstallSection {
                    WantedBy = "unknown"
                }
            };

            Assert.Equal(@"[Unit]
Description=Accelerator

[Service]
WorkingDirectory=/var/apps/accelerator/latest
ExecStart=/var/apps/Accelerator -port 80
Restart=always
RestartSec=10
User=www-user

[Install]
WantedBy=unknown", unit.ToString());
        }


    }
}