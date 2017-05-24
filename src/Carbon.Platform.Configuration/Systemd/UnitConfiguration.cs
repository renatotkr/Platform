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
                WriteSection(sb, "Unit", Unit);
            }

            if (Service != null)
            {
                WriteSection(sb, "Service", Service);
            }

            if (Install != null)
            {
                WriteSection(sb, "Install", Install);
            }
            
            return sb.ToString();
        }
        
        private static void WriteSection(StringBuilder sb, string name, SectionBase section)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine();
            }

            sb.Append("[" + name + "]");

            foreach (var directive in section.GetDirectives())
            {
                sb.AppendLine();

                sb.Append($"{directive.Name}={directive.Value}");
            }
        }
    }
}

// ref: https://www.freedesktop.org/software/systemd/man/systemd.exec.html