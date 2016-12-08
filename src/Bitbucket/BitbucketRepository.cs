using System;
using System.Threading.Tasks;
using System.Net;

using Carbon.Packaging;
using Carbon.Repositories;
using Carbon.Storage;

namespace Bitbucket
{
    public class BitbucketRepository : IRepositoryClient
    {
        private readonly BitbucketClient client;

        public BitbucketRepository(Uri url, NetworkCredential credentials)
        {
            #region Preconditions

            if (url == null)
                throw new ArgumentNullException(nameof(url));

            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));

            #endregion

            // https://bitbucket.org/carbonmade/lefty.git

            var repo = RepositoryInfo.Parse(url.ToString());
            AccountName = repo.AccountName;
            RepositoryName = repo.Name;

            client = new BitbucketClient(credentials);
        }

        public string AccountName { get; }

        public string RepositoryName { get; }

        public BitbucketRepository(string accountName, string repositoryName, NetworkCredential credentials)
        {
            #region Preconditions

            if (accountName == null)
                throw new ArgumentNullException(nameof(accountName));

            if (repositoryName == null)
                throw new ArgumentNullException(nameof(repositoryName));

            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));

            #endregion
        
            AccountName = accountName;
            RepositoryName = repositoryName;

            this.client = new BitbucketClient(credentials);
        }

        public async Task<ICommit> GetCommitAsync(Revision revision)
        {
            var commit = await client.GetCommitAsync(AccountName, RepositoryName, revision.Name).ConfigureAwait(false);

            if (commit == null)
            {
                throw new Exception($"The repository '{RepositoryName}' does not have a reference named '{revision.Path}'");
            }

            return commit;
        }

        public async Task<IPackage> DownloadAsync(Revision revision)
        {
            var stream = await client.GetZipStreamAsync(AccountName, RepositoryName, revision.Name).ConfigureAwait(false);

            return ZipPackage.FromStream(stream, stripFirstLevel: true);
        }
    }
}