﻿using System;

using System.Text;

namespace Carbon.Platform
{
    public struct RepositoryDetails
    {
        public RepositoryDetails(RepositoryHostType host, string accountName, string name, string revision)
        {
            HostType = host;
            AccountName = accountName;
            Name = name;
            Revision = revision;
        }

        public RepositoryHostType HostType { get; }

        public string AccountName { get; }

        public string Name { get; }

        public string Revision { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (HostType != RepositoryHostType.GitHub)
            {
                sb.Append(HostType.ToString().ToLower());
                sb.Append(":");
            }

            sb.Append(AccountName);
            sb.Append("/");
            sb.Append(Name);

            if (Revision != null)
            {
                sb.Append("#");
                sb.Append(Revision);
            }

            return sb.ToString();
        }

        public static RepositoryDetails Parse(string text)
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

            var hostType = RepositoryHostType.GitHub;
            string accountName = null;
            string repositoryName = null;
            string revision = null;

            var i = 0;

            foreach (var part in text.Split(':', '/', '#'))
            {
                if (i == 0)
                {
                    if (hasHost)
                    {
                        hostType = RepositoryHost.Parse(part);
                    }
                    else
                    {
                        hostType = RepositoryHostType.GitHub;
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
                        revision = part;
                    }
                }

                if (i == 3)
                {
                    revision = part;
                }

                i++;
            }

            if (repositoryName.EndsWith(".git"))
            {
                repositoryName = repositoryName.Replace(".git", "");
            }

            return new RepositoryDetails(hostType, accountName, repositoryName, revision);
        }       
    }
}