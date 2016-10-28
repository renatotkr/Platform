using System;

using Carbon.Repositories;

namespace GitHub
{
    public class GetArchiveLinkRequest
    {
        public GetArchiveLinkRequest(string accountName, string repositoryName, Revision revision, ArchiveFormat format = ArchiveFormat.Zipball)
        {
            #region Preconditions

            if (accountName == null)
                throw new ArgumentNullException(nameof(accountName));

            if (repositoryName == null)
                throw new ArgumentNullException(nameof(repositoryName));

            #endregion

            AccountName = accountName;
            RepositoryName = repositoryName;
            Revision = revision;
            Format = format;
        }

        public string AccountName { get; }

        public string RepositoryName { get; }

        public Revision Revision { get; } //   // Optional string - valid Git reference, defaults to master

        public ArchiveFormat Format { get; }

        public string ToPath()
        {
            // GET /repos/:owner/:repo/:archive_format/:ref

            // /repos/user/repo/zipball/dev

            var format = Format.ToString().ToLower();

            return $"/repos/{AccountName}/{RepositoryName}/{format}/{Revision.Name}";
             
        }
    }

    public enum ArchiveFormat
    {
        Tarball,
        Zipball
    }
}