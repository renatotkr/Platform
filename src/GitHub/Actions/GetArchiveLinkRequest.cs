using System;

using Carbon.VersionControl;

namespace GitHub
{
    public class GetArchiveLinkRequest
    {
        public GetArchiveLinkRequest(
            string accountName,
            string repositoryName,
            Revision revision,
            GitArchiveFormat format = GitArchiveFormat.Zipball)
        {
            AccountName     = accountName    ?? throw new ArgumentNullException(nameof(accountName));
            RepositoryName  = repositoryName ?? throw new ArgumentNullException(nameof(repositoryName));
            Revision        = revision;
            Format          = format;
        }

        public string AccountName { get; }

        public string RepositoryName { get; }

        public Revision Revision { get; } //   // Optional string - valid Git reference, defaults to master

        public GitArchiveFormat Format { get; }

        public string ToPath()
        {
            // GET /repos/:owner/:repo/:archive_format/:ref

            // /repos/user/repo/zipball/dev

            var format = Format.ToString().ToLower();

            return $"/repos/{AccountName}/{RepositoryName}/{format}/{Revision.Name}";
             
        }
    }

    public enum GitArchiveFormat
    {
        Tarball = 1,
        Zipball = 2
    }
}