using System;

namespace Carbon.Platform
{
    public static class RepositoryProvider
    {
        public static RepositoryProviderId Parse(string text)
        {
            switch (text.ToLower())
            {
                case "bitbucket":
                case "bitbucket.org": return RepositoryProviderId.BitBucket;

                case "github":
                case "github.com": return RepositoryProviderId.GitHub;

                case "gitlab": return RepositoryProviderId.GitLab;

                default: throw new Exception("Unexpected host: " + text);
            }
        }        
    }

    public enum RepositoryProviderId
    {
        GitHub = 5000,
        BitBucket = 5001,
        GitLab = 5002
    }
}
