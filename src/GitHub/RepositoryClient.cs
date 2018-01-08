using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Packaging;
using Carbon.Storage;
using Carbon.VersionControl;

namespace GitHub
{
    public class RepositoryClient : IRepositoryClient
    {
        private readonly GitHubClient client;

        public RepositoryClient(Uri url, OAuth2Token accessToken)
        {
            #region Preconditions

            if (url == null)
                throw new ArgumentNullException(nameof(url));

            #endregion

            // https://github.com/{orgName}/{repoName}.git

            var info = RevisionSource.Parse(url);
      
            AccountName    = info.AccountName;
            RepositoryName = info.RepositoryName;

            client = new GitHubClient(accessToken);
        }

        public RepositoryClient(string accountName, string repositoryName, OAuth2Token accessToken)
        {
            AccountName    = accountName    ?? throw new ArgumentNullException(nameof(accountName));
            RepositoryName = repositoryName ?? throw new ArgumentNullException(nameof(repositoryName));

            client = new GitHubClient(accessToken);
        }

        public string AccountName { get; }

        public string RepositoryName { get; }

        #region Refs

        public Task<GitRef> GetRefAsync(string refName)
        {
            #region Preconditions

            if (refName == null)
                throw new ArgumentNullException(nameof(refName));

            #endregion

            return client.GetRefAsync(AccountName, RepositoryName, refName);
        }

        public Task<GitRef[]> GetRefsAsync()
        { 
            return client.GetRefsAsync(AccountName, RepositoryName);
        }

        #endregion

        public async Task<ICommit> GetCommitAsync(Revision revision)
        {
            if (revision.Type == RevisionType.Commit)
            {
                return await GetCommitAsync(revision.Name).ConfigureAwait(false);
            }

            var reference = await GetRefAsync(revision.Path).ConfigureAwait(false)
                ?? throw new Exception($"The ref '{revision.Path}' was not found in '{AccountName}/{RepositoryName}'");

            return reference.Object.AsCommit();
        }

        public async Task<ICommit> GetCommitAsync(string sha1)
        {
            return await client.GetCommitAsync(AccountName, RepositoryName, sha1);
        }

        public Task<IList<GitBranch>> GetBranchesAsync()
        {
            return client.GetBranchesAsync(AccountName, RepositoryName);
        }

        public async Task<IPackage> DownloadAsync(Revision revision)
        {
            var request = new GetArchiveLinkRequest(
                accountName    : AccountName, 
                repositoryName : RepositoryName, 
                revision       : revision, 
                format         : GitArchiveFormat.Zipball
            );

            var link = await client.GetArchiveLinkAsync(request).ConfigureAwait(false);

            return await ZipPackage.DownloadAsync(link, stripFirstLevel: true).ConfigureAwait(false);
        }
    }
}