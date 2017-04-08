﻿using System;

namespace Carbon.VersionControl
{
    public class RepositoryProvider : IEquatable<RepositoryProvider>
    {
        // servicename?

        public RepositoryProvider(int id, string name, string domain = null)
        {
            Id    = id;
            Name   = name ?? throw new ArgumentNullException(nameof(name));
            Domain = domain;
        }

        public int Id { get; }

        public string Name { get; }

        public string Domain { get; }

        public static RepositoryProvider Amazon    = new RepositoryProvider(1,    "amazon.codecommit");
        public static RepositoryProvider GitHub    = new RepositoryProvider(1000, "github", "github.com");
        public static RepositoryProvider Bitbucket = new RepositoryProvider(1001, "bitbucket", "bitbucket.org");
        public static RepositoryProvider GitLab    = new RepositoryProvider(1002, "gitlab", "gitlab.com");

        // Code commit urls are region scoped...

        // https://git-codecommit.us-east-2.amazonaws.com/

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

                case "codecommit": return Amazon;

                default: throw new Exception("Unsupported provider: " + text);
            }
        }
    }
}