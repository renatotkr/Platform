using System;
using System.Runtime.Serialization;

namespace Carbon.VersionControl
{
    [DataContract]
    public readonly struct Revision : IEquatable<Revision>
    {
        public static readonly Revision Master = Head("master");

        public Revision(string name, RevisionType type)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
        }

        [DataMember(Name = "name", Order = 1)]
        public string Name { get; }

        [DataMember(Name = "type", Order = 2)]
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

        public override string ToString()
        {
            switch (Type)
            {
                case RevisionType.Commit    : return "commit:" + Name;
                case RevisionType.Tag       : return "tag:"    + Name;
                case RevisionType.Head      : return "head:"   + Name;
                default                     : throw new Exception("Unexpected type:" + Type);
            }
        }

        public static Revision Commit(string sha) =>
            new Revision(sha, RevisionType.Commit);

        public static Revision Tag(string name) =>
            new Revision(name, RevisionType.Tag);

        public static Revision Head(string name) =>
            new Revision(name, RevisionType.Head);
    
        private static readonly char[] seperators = { '/', ':' };

        public static Revision Parse(string text)
        {
            var parts = text.Split(seperators);

            if (parts.Length == 1)
            {
                var name = parts[0];

                if (name.Length == 40 || name.Length == 64) // sha1 (dae86e1950b1277e545cee180551750029cfe735) |sha3
                {
                    return Commit(name);
                }

                return Head(name);
            }
            else if (parts.Length == 2)
            {
                // head:name | heads/name

                var name = parts[1];
                var type = GetType(parts[0]);

                return new Revision(name, type);
            }

            throw new Exception("Unexpected revision format:" + text);

        }

        private static RevisionType GetType(string typeName)
        {
            switch (typeName)
            {
                case "tags"     : return RevisionType.Tag;  
                case "branches" : return RevisionType.Head;
                case "heads"    : return RevisionType.Head; 
                case "tag"      : return RevisionType.Tag; 
                case "branch"   : return RevisionType.Head;
                case "head"     : return RevisionType.Head;
                case "commit"   : return RevisionType.Commit; 
                default         : throw new Exception("Unexpected kind:" + typeName);
            }
        }

        #region Equality

        public bool Equals(Revision other) =>
            Name == other.Name &&
            Type == other.Type;

        #endregion
    }
}