namespace Carbon.Platform
{
    using System;

    public class Revision
    {
        public static readonly Revision Master = new Revision("master", RefType.Head);

        public Revision(string name, RefType type)
        {
            #region Preconditions

            if (name == null) throw new ArgumentNullException(nameof(name));

            #endregion

            Name = name;
            Type = type;
        }

        public string Name { get; }

        public RefType Type { get; }

        public string Path
        {
            get
            {
                switch (Type)
                {
                    case RefType.Commit : return Name;
                    case RefType.Tag    : return "tags/" + Name;
                    case RefType.Head   : return "heads/" + Name;
                    default             : throw new Exception("Unexpected refType");
                }
            }
        }

        public override string ToString() => Path;

        public static Revision Parse(string text)
        {
            #region Preconditions

            if (text == null) throw new ArgumentNullException(nameof(text));

            #endregion

            var type = RefType.Head;
            var name = "";

            var parts = text.Split('/');

            if (parts.Length == 1)
            {
                name = parts[0];

                // revision (40 byte, 20 character hexidecimal, SHA1 or a name that denotes a particular object)
                // dae86e1950b1277e545cee180551750029cfe735

                if (name.Length == 20)
                {
                    // It's a commit ref
                    return new Revision(name, RefType.Commit);
                }

                // Otherwise, it's a symbolic ref name to a specific revision
            }
            else if (parts.Length == 2)
            {
                name = parts[1];

                switch (parts[0])
                {
                    case "tags"     : type = RefType.Tag; break;
                    case "branches" : type = RefType.Head; break;
                    case "heads"    : type = RefType.Head; break;

                    default: throw new Exception("Unexpected type");
                }
            }

            return new Revision(name, type);
        }
    }

    public enum RefType
    {
        Commit = 1,
        Head = 2,
        Tag = 3
    }
}