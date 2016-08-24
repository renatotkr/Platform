using System;
using System.Threading.Tasks;
using System.Net;

namespace Carbon.Packaging.Resolvers
{
    using Bitbucket;

    using GitHub;
    using Platform;

    public class PackageResolver
    {
        public GitCredentials GitHubCredentials { get; set;}

        public NetworkCredential BitBucketCredentials { get; set; }

        public IRepositoryClient GetRepository(RepositoryDetails repository)
        {
            switch (repository.HostType)
            {
                case RepositoryProviderId.BitBucket:
                    return new BitbucketRepository(repository.AccountName, repository.Name, BitBucketCredentials);

                case RepositoryProviderId.GitHub:
                    return new GitRepository(repository.AccountName, repository.Name, GitHubCredentials);

                default: throw new Exception("Unsupported repository provider:" + repository.HostType);
            }
        }

        public Task<Package> GetAsync(RepositoryDetails source)
        {
            var revision = Revision.Parse(source.Revision);

            return GetRepository(source).DownloadAsync(revision);
        }
        
    }
}

// resolve depedencies?