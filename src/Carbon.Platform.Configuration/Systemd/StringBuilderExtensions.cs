using System.Text;

namespace Carbon.Platform.Configuration.Systemd
{
    internal static class StringBuilderExtensions
    {
        public static void WriteSection(this StringBuilder sb, string name, SectionBase section)
        {
            if (sb.Length > 0) sb.AppendLine();

            sb.AppendLine("[" + name + "]");

            foreach (var directive in section.GetDirectives())
            {
                sb.AppendLine($"{directive.Name}={directive.Value}");
            }
        }
    }
}