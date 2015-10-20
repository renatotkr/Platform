namespace Carbon.Platform
{
    using System;
    using System.Text;

    /*
    MAJOR version when you make incompatible API changes,
    MINOR version when you add functionality in a backwards-compatible manner, and
    PATCH version when you make backwards-compatible bug fixes.
    */

    // The tilde can also be used, e.g. ~1.3.15. 
    // This would mean that you want either 1.3.15 or a higher version number that starts with 1.3 (it is in the same minor range).

    public struct Semver
    {
        private static readonly Semver Latest = new Semver(-1, -1, -1);

        public Semver(int major, int minor = -1, int patch = -1)
        {
            Major = major;
            Minor = minor;
            Patch = patch;   
        }

        public int Major { get; }

        public int Minor { get; }

        public int Patch { get; }

        public VersionCategory Level
        {
            get
            {
                if (Minor == -1) return VersionCategory.Major;
                if (Patch == -1) return VersionCategory.Minor;
                else             return VersionCategory.Patch;
            }

        }

        public bool Satisfies(Semver ver)
        {
            return Major >= ver.Major 
                && Minor >= ver.Minor
                && Patch >= ver.Patch;
        }

        // TODO: Equality & Comparision overrides

        public static Semver Parse(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            if (text == "latest") return Latest;

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


        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Major == -1) return "latest";

            sb.Append(Major);
            sb.Append(".");
            sb.Append(Minor == -1 ? "x" : Minor.ToString());
            sb.Append(".");
            sb.Append(Patch == -1 ? "x" : Patch.ToString());

            return sb.ToString();
        }

        public string ToAlignedString()
        {
            var sb = new StringBuilder();

            if (Major == -1) return "latest";

            sb.Append(Major.ToString("000"));
            sb.Append(".");
            sb.Append(Minor == -1 ? "x" : Minor.ToString("000"));
            sb.Append(".");
            sb.Append(Patch == -1 ? "x" : Patch.ToString("0000"));

            return sb.ToString();
        }
    }

    public enum VersionCategory
    {
        Major,
        Minor,
        Patch
    }


    // Dynamo Serialization


    /// 001.001.0000
   
}
