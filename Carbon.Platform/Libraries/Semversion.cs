namespace Carbon.Platform
{
    using System;
    using System.Text;

    /*
    MAJOR version when you make incompatible API changes,
    MINOR version when you add functionality in a backwards-compatible manner, and
    PATCH version when you make backwards-compatible bug fixes.
    */

  
    public struct Semver : IComparable<Semver>, IEquatable<Semver>
    {
        public static readonly Semver Zero = new Semver(0, 0, 0);

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
                else return VersionCategory.Patch;
            }

        }

        public bool Satisfies(Semver ver)
        {
            return GetRange().Contains(ver);
        }

        public SemverRange GetRange()
        {
            switch (Level)
            {
                case VersionCategory.Major: return new SemverRange(new Semver(Major, 0, 0), new Semver(Major, 999, 9999));
                case VersionCategory.Minor: return new SemverRange(new Semver(Major, Minor, 0), new Semver(Major, Minor, 9999));
                case VersionCategory.Patch: return new SemverRange(this, this);

                default: throw new Exception("Unexpected level:" + this.Level);
            }
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
                 patch: (parts.Length == 3 && parts[2] != "x") ? int.Parse(parts[2]) : -1);
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

        public int CompareTo(Semver value)
        {
            if (Major != value.Major)
            {
                return (Major > value.Major) ? 1 : -1;
            }

            if (Minor != value.Minor)
            {
                return (Minor > value.Minor) ? 1 : -1;
            }

            if (Patch != value.Patch)
            {
                return (Patch > value.Patch) ? 1 : -1;
            }

            return 0;
        }

        #region Equality

        public bool Equals(Semver value)
        {
            if ((Major != value.Major) ||
                (Minor != value.Minor) ||
                (Patch != value.Patch))
                return false;

            return true;
        }

        public override bool Equals(object value)
        {
            if (value == null) return false;

            return Equals((Semver)value);
        }

        public override int GetHashCode()
        {
            var hash = Major.GetHashCode();

            hash = CombineHashes(hash, Minor.GetHashCode());
            hash = CombineHashes(hash, Patch.GetHashCode());

            return hash;
        }

        private static int CombineHashes(int h1, int h2)
        {
            return (((h1 << 5) + h1) ^ h2);
        }

        #endregion

        #region Operators

        public static bool operator ==(Semver v1, Semver v2) => v1.Equals(v2);

        public static bool operator !=(Semver v1, Semver v2) => !(v1 == v2);

        public static bool operator <(Semver v1, Semver v2) => v1.CompareTo(v2) < 0;

        public static bool operator <=(Semver v1, Semver v2) => v1.CompareTo(v2) <= 0;

        public static bool operator >(Semver v1, Semver v2) => v2 < v1;

        public static bool operator >=(Semver v1, Semver v2) => v2 <= v1;

        #endregion
    }

    // The tilde can also be used, e.g. ~1.3.15. 
    // This would mean that you want either 1.3.15 or a higher version number that starts with 1.3 (it is in the same minor range).

    public struct SemverRange
    {
        public static readonly SemverRange Latest = new SemverRange(Semver.Zero, new Semver(999, 999, 9999));

        public SemverRange(Semver start, Semver end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(Semver ver)
        {
            return ver >= Start && ver <= End;
        }

        public Semver Start { get; }

        public Semver End { get; }

        // TODO: Parse
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
