using System;
using System.Threading.Tasks;
using System.Net;

using Carbon.Packaging;
using Carbon.Storage;
using Carbon.VersionControl;

namespace Bitbucket
{
    public class BitbucketRepository : IRepositoryClient
    {
        private readonly BitbucketClient client;

        // https://bitbucket.org/{accontName}/{repositoryName}.git

        public BitbucketRepository(Uri url, NetworkCredential credential)
        {
            #region Preconditions

            if (url == null)
                throw new ArgumentNullException(nameof(url));

            if (credential == null)
                throw new ArgumentNullException(nameof(credential));

            #endregion

            var repo = RevisionSource.Parse(url);

            AccountName    = repo.AccountName;
            RepositoryName = repo.RepositoryName;

            client = new BitbucketClient(credential);
        }

        public string AccountName { get; }

        public string RepositoryName { get; }

        public BitbucketRepository(
            string accountName, 
            string repositoryName, 
            NetworkCredential credential)
        {
            #region Preconditions

            if (credential == null)
                throw new ArgumentNullException(nameof(credential));

            #endregion
        
            AccountName    = accountName ?? throw new ArgumentNullException(nameof(accountName));
            RepositoryName = repositoryName ?? throw new ArgumentNullException(nameof(repositoryName));

            this.client = new BitbucketClient(credential);
        }

        public async Task<ICommit> GetCommitAsync(Revision revision)
        {
            return await client.GetCommitAsync(AccountName, RepositoryName, revision.Name).ConfigureAwait(false)
                ?? throw new Exception($"No ref named '{revision.Path}' exists in '{RepositoryName}'");
        }

        public async Task<IPackage> DownloadAsync(Revision revision)
        {
            var stream = await client.GetZipStreamAsync(
                accountName     : AccountName, 
                repositoryName  : RepositoryName, 
                revision        : revision.Name
            ).ConfigureAwait(false);

            return ZipPackage.FromStream(stream, stripFirstLevel: true);
        }
    }
}