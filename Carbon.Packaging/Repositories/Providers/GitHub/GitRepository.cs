using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Platform;
using Carbon.Packaging;

namespace GitHub
{
    public class GitRepository : IRepositoryClient
    {
        private readonly string accountName;
        private readonly string repositoryName;

        private readonly GitHubClient client;

        public GitRepository(Uri url, GitHubCredentials credentials)
        {
            // https://github.com/orgName/repoName.git

            var path = url.AbsolutePath.Replace(".git", "");
            var split = path.Trim('/').Split('/');

            this.accountName = split[0];
            this.repositoryName = split[1];

            this.client = new GitHubClient(credentials);
        }

        public GitRepository(string accountName, string repositoryName, GitHubCredentials credentials)
        {
            #region Preconditions

            if (accountName == null) throw new ArgumentNullException(nameof(accountName));
            if (repositoryName == null) throw new ArgumentNullException(nameof(repositoryName));

            #endregion

            this.accountName = accountName;
            this.repositoryName = repositoryName;

            this.client = new GitHubClient(credentials);
        }

        #region Refs

        public Task<GitRef> GetRef(string refName)
        {
            return client.GetRef(accountName, repositoryName, refName);
        }

        public Task<GetRefsResult> GetRefs()
        {
            return client.GetRefs(accountName, repositoryName);
        }

        #endregion

        public async Task<ICommit> GetCommitAsync(Revision revision)
        {
            var reference = await GetRef(revision.Path).ConfigureAwait(false);

            if (reference == null)
            {
                throw new Exception($"The repository '{repositoryName}' does not have a reference named '{revision.Path}'");
            }

            return reference.Object.ToCommit();
        }

        public Task<IList<GitBranch>> GetBranches()
        {
            return client.GetBranches(accountName, repositoryName);
        }

        public Task TagAsync(ICommit commit, string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Package> DownloadAsync(Revision revision)
        {
            var request = new GetArchiveLinkRequest(accountName, repositoryName, revision, ArchiveFormat.Zipball);

            var link = await client.GetArchiveLink(request).ConfigureAwait(false);

            return await ZipPackage.DownloadAsync(link, stripFirstLevel: true).ConfigureAwait(false);
        }
    }
}