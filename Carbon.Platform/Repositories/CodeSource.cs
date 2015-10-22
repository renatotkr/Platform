namespace Carbon.Platform
{
    using System;
    using System.Text;

    public class CodeSource
    {
        public CodeHost Host { get; set; }

        public string AccountName { get; set; }

        public string RepositoryName { get; set; }

        public string Revision { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Host != CodeHost.GitHub)
            {
                sb.Append(Host.ToString().ToLower());
                sb.Append(":");
            }

            sb.Append(AccountName);
            sb.Append("/");
            sb.Append(RepositoryName);

            if (Revision != null)
            {
                sb.Append("#");
                sb.Append(Revision);
            }

            return sb.ToString();
        }

        public static CodeSource Parse(string text)
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

            var result = new CodeSource();          

            var i = 0;

            foreach (var part in text.Split(':', '/', '#'))
            {
                if (i == 0)
                {
                    if (hasHost)
                    {
                        result.Host = GetHost(part);
                    }
                    else
                    {
                        result.Host = CodeHost.GitHub;
                        result.AccountName = part;
                    }
                }

                if (i == 1)
                {
                    if (result.AccountName == null)
                    {
                        result.AccountName = part;
                    }
                    else
                    {
                        result.RepositoryName = part;
                    }
                }

                if (i == 2)
                {
                    if (result.RepositoryName == null)
                    {
                        result.RepositoryName = part;
                    }
                    else
                    {
                        result.Revision = part;
                    }
                }

                if (i == 3)
                {
                    result.Revision = part;
                }

                i++;
            }

            if (result.RepositoryName.EndsWith(".git"))
            {
                result.RepositoryName = result.RepositoryName.Replace(".git", "");
            }

            return result;
        }

        private static CodeHost GetHost(string text)
        {
            switch (text.ToLower())
            {
                case "bitbucket":
                case "bitbucket.org": return CodeHost.BitBucket;

                case "github":
                case "github.com": return CodeHost.GitHub;

                case "gist": return CodeHost.Gist;

                case "gitlab": return CodeHost.GitLab;

                default: throw new Exception("Unexpected host: " + text);

            }
        }
    }

 
    
    public enum CodeHost
    {
        GitHub,
        BitBucket,
        Gist,
        GitLab
    }
}


// visionmedia/mocha#4727d357ea

/*
git://github.com/user/project.git#commit-ish
*/