using System;
using System.Linq;
using Carbon.Extensions;

namespace Carbon.Platform
{
    public sealed class DomainName
    {
        private string[] labels;

        public DomainName(string name)
            : this(name.Split(Seperators.Period)) { }

        public DomainName(string[] labels)
        {
            this.labels = labels ?? throw new ArgumentNullException(nameof(labels));

            if (labels.Length == 0)
                throw new Exception("Must have at least 1 label");

            foreach (var label in labels)
            {
                if (label.Length == 0)
                    throw new Exception("label may not be empty");

                if (label.Length > 63)
                {
                    throw new Exception("label must 63 characters or fewer");
                }
                
                // TODO: Validate the name against the DOMAIN NAMES specification
                // https://tools.ietf.org/html/rfc1035
            }
        }

        public string[] Labels => labels;

        
        // ReverseDomainNameNotation
        // https://en.wikipedia.org/wiki/Reverse_domain_name_notation
        
        // com/domain/superawesome
        public string Path => string.Join("/", labels.Reverse());
    }
}
