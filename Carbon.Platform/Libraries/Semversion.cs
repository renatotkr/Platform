namespace Carbon.Libraries
{
    /*
    MAJOR version when you make incompatible API changes,
    MINOR version when you add functionality in a backwards-compatible manner, and
    PATCH version when you make backwards-compatible bug fixes.
    */

    // The tilde can also be used, e.g. ~1.3.15. 
    // This would mean that you want either 1.3.15 or a higher version number that starts with 1.3 (it is in the same minor range).

    public struct Semver
    {
        public Semver(int major, int minor = -1, int patch = -1)
        {
            Major = major;
            Minor = minor;
            Patch = patch;   
        }

        public int Major { get; }

        public int Minor { get; }

        public int Patch { get; }

        public MatchLevel Level
        {
            get
            {
                if (Minor == -1) return MatchLevel.Major;
                if (Patch == -1) return MatchLevel.Minor;
                else             return MatchLevel.Patch;
            }

        }

        public Semver Parse(string text)
        {
            var parts = text.Split('.');

            // 2.0.1
            // 2.1.x
            // 2.x.x
            
            // TODO: check for ~

            return new Semver(
                 major: int.Parse(parts[0]),
                 minor: (parts[1] != "x") ? int.Parse(parts[1]) : -1,
                 patch: (parts.Length == 3 && parts[2] != "x") ? int.Parse(parts[2]) : -1
             );
        }
    }

    public enum MatchLevel
    {
        Major,
        Minor,
        Patch
    }
}
