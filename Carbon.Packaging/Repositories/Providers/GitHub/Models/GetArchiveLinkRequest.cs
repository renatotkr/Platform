using System;

using Carbon.Storage;

namespace GitHub
{
    public class GetArchiveLinkRequest
    {
        private readonly string accountName;
        private readonly string repositoryName;
        private readonly Revision revision;             // Optional string - valid Git reference, defaults to master
        private readonly ArchiveFormat format;

        public GetArchiveLinkRequest(string accountName, string repositoryName,
            Revision revision, ArchiveFormat format = ArchiveFormat.Zipball)
        {
            #region Preconditions

            if (accountName == null)
                throw new ArgumentNullException(nameof(accountName));

            if (repositoryName == null)
                throw new ArgumentNullException(nameof(repositoryName));

            #endregion

            this.accountName = accountName;
            this.repositoryName = repositoryName;
            this.revision = revision;
            this.format = format;
        }

        public string ToPath()
        {
            // GET /repos/:owner/:repo/:archive_format/:ref

            // /repos/user/repo/zipball/dev

            var result = string.Format("/repos/{0}/{1}/{2}/{3}",
                /*0*/ accountName,
                /*1*/ repositoryName,
                /*2*/ format.ToString().ToLower(),
                /*3*/ revision.Name
            );

            return result;
        }
    }

    public enum ArchiveFormat
    {
        Tarball,
        Zipball
    }
}