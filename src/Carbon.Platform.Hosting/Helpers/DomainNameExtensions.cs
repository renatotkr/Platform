using System;

using Carbon.Net.Dns;

namespace Carbon.Platform
{
    public static class DomainNameExtensions
    {
        public static string GetPath(in this DomainName name, int level)
        {
            if (level == 1) return name.Tld;

            if (level == 2) return name.Tld + "/" + name.Sld;
            
            var labels = new string[level];

            Array.Copy(name.Labels, name.Labels.Length - level, labels, 0, labels.Length);

            return new DomainName(labels).Path;
        }
    }
}
    
// ReverseDomainNameNotation
// https://en.wikipedia.org/wiki/Reverse_domain_name_notation