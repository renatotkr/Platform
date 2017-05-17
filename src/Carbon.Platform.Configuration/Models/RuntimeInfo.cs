using System;

namespace Carbon.Platform.Configuration
{
    public struct RuntimeInfo
    {
        public RuntimeInfo(string name, string version)
        {
            Name    = name    ?? throw new ArgumentNullException(nameof(name));
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }

        public string Name { get; }

        public string Version { get; }

        public override string ToString() => Name + "/" + Version;

        public static RuntimeInfo Parse(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            #endregion

            var parts = text.Split('/', '@');

            return new RuntimeInfo(parts[0], parts[1]);
        }
    }

    public class RuntimeNames
    {
        public const string NodeJS     = "nodejs";
        public const string NetCoreApp = "netcoreapp";
        public const string Python     = "python";
    }
}

// nodejs | nodejs@4.3 | nodejs@6.10 | nodejs@8.0
// java@8
// python@2.7 | python@3.6
// dotnetcore@1.0 | dotnetcore@1.1 | dotnetcore@2.0
