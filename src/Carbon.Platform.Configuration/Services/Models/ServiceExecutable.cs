using System;

namespace Carbon.Platform.Configuration
{
    // e.g. Accelerator -port 8000

    public class ServiceExecutable
    {
        public ServiceExecutable(string name, string arguments = null)
        {
            Name      = name ?? throw new ArgumentException(nameof(name));
            Arguments = arguments;
        }

        public string Name { get; }

        public string Arguments { get; }

        // IDictionary<string, string> Environment

        // 

        public static ServiceExecutable Parse(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            #endregion

            var parts = text.Split(new[] { ' ' }, 2);

            if (parts.Length == 2)
            {
                return new ServiceExecutable(parts[0], parts[1]);
            }

            return new ServiceExecutable(parts[0]);
        }

        public override string ToString()
        {
            if (Arguments == null) return Name;

            return Name + " " + Arguments;
        }
    }
}
