namespace Carbon.Platform.Configuration
{
    public readonly struct PackageInfo
    {
        public PackageInfo(string name, string version = null)
        {
            Name    = name;
            Version = version;
        }

        public string Name { get; }

        public string Version { get; }

        // e.g. nodejs | nodejs@8.0.0 || nodejs/8.0.0
        public static PackageInfo Parse(string text)
        {
            if (text.Contains("@"))
            {
                var parts = text.Split('@');

                return new PackageInfo(parts[0], parts[1]);
            }

            return new PackageInfo(text);
        }

        public override string ToString() => Name;
    }
}

// awscli nginx libunwind8 libcurl4-openssl-dev

// yum or apt

// sudo apt install gparted=0.16.1-1