using System.Collections.Generic;
using System.Linq;

namespace Carbon.Platform.Configuration.Systemd
{
    public class SectionBase
    {
        private readonly Dictionary<string, Directive> directives = new Dictionary<string, Directive>();

        public IEnumerable<Directive> GetDirectives()
        {
            return directives.Values.OrderBy(d => d.Order);
        }

        protected void Set(string name, string value, int order = 100)
        {
            if (value == null)
            {
                directives.Remove(name);
            }
            else
            {
                directives[name] = new Directive(name, value, order);
            }
        }

        protected string Get(string name)
        {
            return directives.TryGetValue(name, out var directive) ? directive.Value : null;
        }

        protected int? GetInteger(string name)
        {
            if (directives.TryGetValue(name, out var directive))
            { 
                return int.Parse(directive.Value);
            }

            return null;
        }

        protected void SetInteger(string name, int? value, int order = 100)
        {
            if (value == null)
            {
                directives.Remove(name);
            }
            else
            {
                directives[name] = new Directive(name, value.ToString(), order);
            }
        }
    }
}
