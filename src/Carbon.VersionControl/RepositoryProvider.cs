using System;

namespace Carbon.VersionControl
{
    public class RepositoryProvider : IEquatable<RepositoryProvider>
    {
        public RepositoryProvider(int id, string name, string domain = null)
        {
            Id     = id;
            Name   = name ?? throw new ArgumentNullException(nameof(name));
            Domain = domain;
        }

        public int Id { get; }

        public string Name { get; }

        public string Domain { get; }

        public static readonly RepositoryProvider CodeCommit = new RepositoryProvider(2,    "aws",       "amazonaws.com");
        public static readonly RepositoryProvider GitHub     = new RepositoryProvider(6000, "github",    "github.com");
        public static readonly RepositoryProvider Bitbucket  = new RepositoryProvider(6001, "bitbucket", "bitbucket.org");
        public static readonly RepositoryProvider GitLab     = new RepositoryProvider(6002, "gitlab",    "gitlab.com");

        // Code commit urls are region scoped...

        // https://git-codecommit.us-east-2.amazonaws.com/

        #region Equality

        public bool Equals(RepositoryProvider other) =>
            Name == other.Name && 
            Domain  == other.Domain;

        public override bool Equals(object obj)
        {
            return obj is RepositoryProvider other && Equals(other);
        }

        public static bool operator ==(RepositoryProvider rhs, RepositoryProvider b)
        {
            return rhs.Equals(b);
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
            #region Preconditions

            if (text == null) throw new ArgumentNullException(nameof(text));

            #endregion

            switch (text.ToLower())
            {
                case "bitbucket":
                case "bitbucket.org": return Bitbucket;

                case "github":
                case "github.com": return GitHub;

                case "gitlab": return GitLab;

                case "codecommit": return CodeCommit;

                default: throw new Exception("Unsupported git provider: " + text);
            }
        }
    }
}