using System;

namespace Carbon.VersionControl
{
    public struct Revision
    {
        public static readonly Revision Master = new Revision("master", RevisionType.Head);

        public Revision(string name, RevisionType type)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
        }

        public string Name { get; }

        public RevisionType Type { get; }

        public string Path
        {
            get
            {
                switch (Type)
                {
                    case RevisionType.Commit : return Name;
                    case RevisionType.Tag    : return "tags/" + Name;
                    case RevisionType.Head   : return "heads/" + Name;
                    default                  : throw new Exception("Unexpected type:" + Type);
                }
            }
        }

        public override string ToString() => Path;

        public static Revision Commit(string sha)
        {
            return new Revision(sha, RevisionType.Commit);
        }

        public static Revision Tag(string name)
        {
            return new Revision(name, RevisionType.Tag);
        }

        public static Revision Head(string name)
        {
            return new Revision(name, RevisionType.Head);
        }

        private static readonly char[] forwardSlash = { '/' };

        public static Revision Parse(string text)
        {
            #region Preconditions

            if (text == null) throw new ArgumentNullException(nameof(text));

            #endregion

            var type = RevisionType.Head;
            string name = null;

            var parts = text.Split(forwardSlash);

            if (parts.Length == 1)
            {
                name = parts[0];

                // revision (40 byte, 20 character hexidecimal, SHA1 or a name that denotes a particular object)
                // dae86e1950b1277e545cee180551750029cfe735

                if (name.Length == 20)
                {
                    // It's a commit ref
                    return new Revision(name, RevisionType.Commit);
                }

                // Otherwise, it's a symbolic ref name to a specific revision
            }
            else if (parts.Length == 2)
            {
                name = parts[1];

                switch (parts[0])
                {
                    case "tags"     : type = RevisionType.Tag; break;
                    case "branches" : type = RevisionType.Head; break;
                    case "heads"    : type = RevisionType.Head; break;

                    default: throw new Exception("Unexpected kind:" + parts[0]);
                }
            }

            return new Revision(name, type);
        }
    }
}
