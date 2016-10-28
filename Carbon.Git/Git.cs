using System;
using System.Threading.Tasks;
using System.Net;

using Bitbucket;
using GitHub;

namespace Carbon.Packaging
{
    using Repositories;

    public static class Git
    {
        public static Task<Package> GetPackageAsync(RepositoryInfo source, GitCredentials credentials)
        {
            return GetRepository(source, credentials).DownloadAsync(source.Revision.Value);
        }

        #region Helpers

        private static IRepositoryClient GetRepository(RepositoryInfo repository, GitCredentials credentials)
        {
            switch (repository.Provider)
            {
                case RepositoryProviderId.Bitbucket:
                    return new BitbucketRepository(repository.AccountName, repository.Name, credentials.BitBucketCredentials);

                case RepositoryProviderId.GitHub:
                    return new GitRepository(repository.AccountName, repository.Name, credentials.GitHubCredentials);

                default: throw new Exception("Unsupported repository provider:" + repository.Provider);
            }
        }
        
        #endregion

    }

    // Union hack...
    public class GitCredentials
    {
        public GitHubCredentials GitHubCredentials { get; set; }

        public NetworkCredential BitBucketCredentials { get; set; }
    }
}

// resolve depedencies?