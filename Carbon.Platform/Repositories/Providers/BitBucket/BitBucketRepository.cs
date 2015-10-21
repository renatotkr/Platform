namespace Bitbucket
{
    using System;
    using System.Threading.Tasks;

    using Carbon.Platform;

    public class BitbucketRepository : IRepositoryClient
    {
        private readonly string accountName;
        private readonly string repositoryName;

        private readonly BitbucketClient client;

        public BitbucketRepository(Uri url, string userName, string password)
        {
            #region Preconditions

            if (url == null) throw new ArgumentNullException(nameof(url));
            if (userName == null) throw new ArgumentNullException(nameof(userName));
            if (password == null) throw new ArgumentNullException(nameof(password));

            #endregion

            // https://bitbucket.org/carbonmade/lefty.git

            var path = url.AbsolutePath.Replace(".git", "");
            var split = path.Trim('/').Split('/');

            this.accountName = split[0];
            this.repositoryName = split[1];

            this.client = new BitbucketClient(userName, password);
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