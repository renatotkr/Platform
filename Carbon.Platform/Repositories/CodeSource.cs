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
                sb.Append(Host.ToString());
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
            var result = new CodeSource();          

            var i = 0;

            foreach (var part in text.Split(':', '/', '#'))
            {
                if (i == 0)
                {
                    if (text.Contains(":"))
                    {
                        result.Host = (CodeHost)Enum.Parse(typeof(CodeHost), part, true);
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

            return result;
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


// git://github.com/carbonmade/backend#123123123
// visionmedia/mocha#4727d357ea