using System.Linq;
using Carbon.Net.Dns;

namespace Carbon.Platform
{
    public static class FqdnExtensions
    {
        // com/domain/superawesome
        public static string GetPath(this Fqdn fqdn)
        {
            return string.Join("/", fqdn.Labels.Reverse());
        }
    }
}
    
// ReverseDomainNameNotation
// https://en.wikipedia.org/wiki/Reverse_domain_name_notation
