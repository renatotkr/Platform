namespace Bitbucket
{
    using System;
    using System.Threading.Tasks;
    using System.Net;

    using Carbon.Platform;

    public class BitbucketRepository : IRepositoryClient
    {
        private readonly string accountName;
        private readonly string repositoryName;

        private readonly BitbucketClient client;

        public BitbucketRepository(Uri url, NetworkCredential credentials)
        {
            #region Preconditions

            if (url == null)            throw new ArgumentNullException(nameof(url));
            if (credentials == null)    throw new ArgumentNullException(nameof(credentials));

            #endregion

            // https://bitbucket.org/carbonmade/lefty.git

            var path = url.AbsolutePath.Replace(".git", "");
            var split = path.Trim('/').Split('/');

            this.accountName = split[0];
            this.repositoryName = split[1];

            this.client = new BitbucketClient(credentials);
        }

        public BitbucketRepository(string accountName, string repositoryName, NetworkCredential credentials)
        {
            #region Preconditions

            if (accountName == null)    throw new ArgumentNullException(nameof(accountName));
            if (repositoryName == null) throw new ArgumentNullException(nameof(repositoryName));
            if (credentials == null)    throw new ArgumentNullException(nameof(credentials));

            #endregion

            // https://bitbucket.org/carbonmade/lefty.git

        
            this.accountName = accountName;
            this.repositoryName = repositoryName;

            this.client = new BitbucketClient(credentials);
        }

        public async Task<ICommit> GetCommitAsync(Revision revision)
        {
            var commit = await client.GetCommit(accountName, repositoryName, revision.Name).ConfigureAwait(false);

            if (commit == null)
            {
                throw new Exception($"The repository '{repositoryName}' does not have a reference named '{revision.Path}'");
            }

            return commit;
        }


        public Task TagAsync(ICommit commit, string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Package> DownloadAsync(Revision revision)
        {
            var stream = await client.GetZipStream(accountName, repositoryName, revision.Name).ConfigureAwait(false);

            return ZipPackage.FromStream(stream, stripFirstLevel: true);
        }
    }
}