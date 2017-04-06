using System;

namespace Carbon.VersionControl
{
    public class RepositoryProvider : IEquatable<RepositoryProvider>
    {
        // servicename?

        public RepositoryProvider(string name, string domain = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Domain = domain;
        }

        // TODO: int Id

        public string Name { get; }

        public string Domain { get; }

        public static RepositoryProvider GitHub    = new RepositoryProvider("github", "github.com");
        public static RepositoryProvider Bitbucket = new RepositoryProvider("bitbucket", "bitbucket.org");
        public static RepositoryProvider GitLab    = new RepositoryProvider("gitlab", "gitlab.com");
        public static RepositoryProvider Amazon    = new RepositoryProvider("amazon.codecommit");

        #region Equality

        public bool Equals(RepositoryProvider other)
        {
            return Name == other?.Name && Domain == other?.Domain;
        }

        public override bool Equals(object obj)
        {
            return (obj as RepositoryProvider)?.Equals(this) == true;
        }

        public static bool operator ==(RepositoryProvider a, RepositoryProvider b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(RepositoryProvider a, RepositoryProvider b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        #endregion

        public static RepositoryProvider Parse(string text)
        {
            switch (text.ToLower())
            {
                case "bitbucket":
                case "bitbucket.org": return Bitbucket;

                case "github":
                case "github.com": return GitHub;

                case "gitlab": return GitLab;

                case "codecommit": return Amazon;

                default: throw new Exception("Unsupported provider: " + text);
            }
        }
    }
}