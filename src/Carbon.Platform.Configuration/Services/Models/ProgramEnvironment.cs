using System;
using System.Collections;
using System.Collections.Generic;

namespace Carbon.Platform.Configuration
{
    public class ProgramEnvironment : IEnumerable<KeyValuePair<string, string>>
    {
        public ProgramEnvironment()
        {
            Variables = new Dictionary<string, string>();
        }

        public ProgramEnvironment(IDictionary<string, string> variables)
        {
            Variables = variables ?? throw new ArgumentNullException(nameof(variables));
        }

        public void Add(string key, string value)
        {
            Variables[key] = value;
        }

        public IDictionary<string, string> Variables { get; set; }

        #region IEnumerable

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return Variables.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Variables.GetEnumerator();
        }

        #endregion
    }
}