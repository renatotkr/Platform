﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Packaging;
using Carbon.Storage;
using Carbon.Repositories;

namespace GitHub
{
    public class GitHubRepository : IRepositoryClient
    {
        private readonly GitHubClient client;

        public GitHubRepository(Uri url, OAuth2Token credentials)
        {
            // https://github.com/orgName/repoName.git

            var info = RepositoryInfo.Parse(url.ToString());
      
            AccountName = info.AccountName;
            RepositoryName = info.Name;

            client = new GitHubClient(credentials);
        }

        public GitHubRepository(string accountName, string repositoryName, OAuth2Token credentials)
        {
            #region Preconditions

            if (accountName == null) throw new ArgumentNullException(nameof(accountName));
            if (repositoryName == null) throw new ArgumentNullException(nameof(repositoryName));

            #endregion

            AccountName = accountName;
            RepositoryName = repositoryName;

            client = new GitHubClient(credentials);
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

            return client.GetRef(AccountName, RepositoryName, refName);
        }

        public Task<GitRef[]> GetRefsAsync()
            => client.GetRefs(AccountName, RepositoryName);

        #endregion

        public async Task<ICommit> GetCommitAsync(Revision revision)
        {
            var reference = await GetRefAsync(revision.Path).ConfigureAwait(false);

            if (reference == null)
            {
                throw new Exception($"The repository '{RepositoryName}' does not have a reference named '{revision.Path}'");
            }

            return reference.Object.ToCommit();
        }

        public Task<IList<GitBranch>> GetBranchesAsync()
            => client.GetBranches(AccountName, RepositoryName);

        public async Task<IPackage> DownloadAsync(Revision revision)
        {
            var request = new GetArchiveLinkRequest(AccountName, RepositoryName, revision, format: ArchiveFormat.Zipball);

            var link = await client.GetArchiveLink(request).ConfigureAwait(false);

            return await ZipPackage.FetchAsync(link, stripFirstLevel: true).ConfigureAwait(false);
        }
    }
}