using System.Text;

using Carbon.Net.Dns;

namespace Carbon.Platform
{
    public static class DomainNameExtensions
    {
        public static string GetPath(this DomainName name, int level)
        {
            if (level == 1) return name.Tld;

            var sb = new StringBuilder();

            // enumerate the array backwards
            for (var i = name.Labels.Length - 1; i >= name.Labels.Length - level; i--)
            {
                var label = name.Labels[i];

                if (sb.Length > 0) sb.Append("/");

                sb.Append(name.Labels[i]);
            }

            return sb.ToString();
        }
    }
}
    
// ReverseDomainNameNotation
// https://en.wikipedia.org/wiki/Reverse_domain_name_notation
