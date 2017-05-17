using System.Text;

namespace Carbon.Platform.Configuration.Systemd
{
    public class UnitConfiguration
    {
        // [Unit]
        public UnitSection Unit { get; set; }

        // [Service]
        public ServiceSection Service { get; set; }

        // [Install]
        public InstallSection Install { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Unit != null)
            {
                sb.WriteSection("Unit", Unit);
            }

            if (Service != null)
            {
                sb.WriteSection("Service", Unit);
            }

            if (Install != null)
            {
                sb.WriteSection("Install", Unit);
            }

            return sb.ToString();
        }
    }
}

// https://www.digitalocean.com/community/tutorials/understanding-systemd-units-and-unit-files

/*
[Unit]
Description=Accelerator

[Service]
WorkingDirectory=/var/apps/accelerator
ExecStart=/var/apps/accelerator/Accelerator
Restart=always
RestartSec=10 # Restart service after 10 seconds if dotnet service crashes
SyslogIdentifier=accelerator
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production 

[Install]
WantedBy=multi-user.target
*/
