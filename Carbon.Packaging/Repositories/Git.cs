using System;
using System.Threading.Tasks;
using System.Net;

using Bitbucket;
using GitHub;

namespace Carbon.Packaging
{
    using Platform;

    public static class Git
    {
        public static Task<Package> GetPackageAsync(RepositoryDetails source, GitCredentials credentials)
        {
            var revision = Revision.Parse(source.Revision);

            return GetRepository(source, credentials).DownloadAsync(revision);
        }

        #region Helpers

        private static IRepositoryClient GetRepository(RepositoryDetails repository, GitCredentials credentials)
        {
            switch (repository.Provider)
            {
                case RepositoryProviderId.BitBucket:
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