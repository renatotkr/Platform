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

        public IRepositoryClient GetRepository(RepositoryInfo source)
        {
            switch (source.HostingService)
            {
                case RepositoryHostType.BitBucket:
                    return new BitbucketRepository(source.AccountName, source.RepositoryName, BitBucketCredentials);

                case RepositoryHostType.GitHub:
                    return new GitRepository(source.AccountName, source.RepositoryName, GitHubCredentials);

                default: throw new Exception("Unexpected source type:" + source.HostingService);
            }
        }

        public Task<Package> GetAsync(RepositoryInfo source)
        {
            var revision = Revision.Parse(source.Revision);

            return GetRepository(source).DownloadAsync(revision);
        }
        
    }
}

// resolve depedencies?