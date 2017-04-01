using System;

namespace Carbon.Repositories
{
    public static class RepositoryProvider
    {
        public static RepositoryProviderId Parse(string text)
        {
            switch (text.ToLower())
            {
                case "bitbucket":
                case "bitbucket.org": return RepositoryProviderId.Bitbucket;

                case "github":
                case "github.com": return RepositoryProviderId.GitHub;

                case "gitlab": return RepositoryProviderId.GitLab;

                case "codecommit": return RepositoryProviderId.Amazon;

                default: throw new Exception("Unsupported provider: " + text);
            }
        }
    }
}