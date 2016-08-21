using System;

namespace Carbon.Platform
{
    public static class RepositoryHost
    {
        public static RepositoryHostType Parse(string text)
        {
            switch (text.ToLower())
            {
                case "bitbucket":
                case "bitbucket.org": return RepositoryHostType.BitBucket;

                case "github":
                case "github.com": return RepositoryHostType.GitHub;

                case "gist": return RepositoryHostType.Gist;

                case "gitlab": return RepositoryHostType.GitLab;

                default: throw new Exception("Unexpected host: " + text);
            }
        }

        
    }

    public enum RepositoryHostType
    {
        GitHub,
        BitBucket,
        Gist,
        GitLab
    }
}
