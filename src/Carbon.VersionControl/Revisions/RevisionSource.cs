using System;
using System.Text;

namespace Carbon.VersionControl
{
    public class RevisionSource
    {
        public RevisionSource(
            RepositoryProvider provider,
            string accountName,
            string name,
            Revision? revision)
        {
            Provider = provider;
            AccountName = accountName;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Revision = revision;
        }

        public RepositoryProvider Provider { get; }

        public string AccountName { get; }

        public string Name { get; }

        public Revision? Revision { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Provider.Name != "github")
            {
                sb.Append(Provider.Name);
                sb.Append(":");
            }

            sb.Append(AccountName);
            sb.Append("/");
            sb.Append(Name);

            if (Revision != null)
            {
                sb.Append("#");
                sb.Append(Revision.Value.Name);
            }

            return sb.ToString();
        }

        public static RevisionSource Parse(string text)
        {
            #region Preconditions

            if (text == null) throw new ArgumentNullException(nameof(text));

            #endregion

            var hasHost = text.Contains(":");

            if (text.Contains("://"))
            {
                // Strip off the protocal
                text = text.Substring(text.IndexOf("://") + 3);
            }

            var hostType = RepositoryProvider.GitHub;
            string accountName = null;
            string repositoryName = null;
            Revision? revision = null;

            var i = 0;

            foreach (var part in text.Split(':', '/', '#'))
            {
                if (i == 0)
                {
                    if (hasHost)
                    {
                        hostType = RepositoryProvider.Parse(part);
                    }
                    else
                    {
                        hostType = RepositoryProvider.GitHub;
                        accountName = part;
                    }
                }

                if (i == 1)
                {
                    if (accountName == null)
                    {
                        accountName = part;
                    }
                    else
                    {
                        repositoryName = part;
                    }
                }

                if (i == 2)
                {
                    if (repositoryName == null)
                    {
                        repositoryName = part;
                    }
                    else
                    {
                        revision = VersionControl.Revision.Parse(part);
                    }
                }

                if (i == 3)
                {
                    revision = VersionControl.Revision.Parse(part);
                }

                i++;
            }

            if (repositoryName.EndsWith(".git"))
            {
                repositoryName = repositoryName.Replace(".git", "");
            }

            return new RevisionSource(hostType, accountName, repositoryName, revision);
        }
    }
}
