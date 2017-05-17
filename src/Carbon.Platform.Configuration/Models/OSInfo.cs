using System;

namespace Carbon.Platform.Configuration
{
    public struct OSInfo
    {
        public OSInfo(string name, string version)
        {
            Name    = name ?? throw new ArgumentNullException(nameof(name));
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }

        public string Name { get; }

        public string Version { get; }

        public override string ToString() => Name + "/" + Version;

        public static OSInfo Parse(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            #endregion

            var parts = text.Split('/');

            return new OSInfo(parts[0], parts[1]);
        }
    }
}
