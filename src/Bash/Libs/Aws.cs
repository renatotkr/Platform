using System;
using System.Text;

namespace Bash.Commands
{
    public static class Aws
    {
        public static class S3
        {
            public static Command Copy(
                string source, 
                string target, 
                AwsOptions options = AwsOptions.None, 
                bool sudo = false)
            {
                #region Preconditions

                if (source == null)
                    throw new ArgumentNullException(nameof(source));

                #endregion

                // sudo aws s3 cp --quiet {source} {target}

                var sb = new StringBuilder("aws s3 cp ");

                if (options.HasFlag(AwsOptions.Quiet))
                {
                    sb.Append("--quiet ");
                }

                sb.Append(source);

                if (target != null)
                {
                    sb.Append(" " + target);
                }
                    
                return new Command(sb.ToString(), sudo);
            }
        }
    }

    [Flags]
    public enum AwsOptions
    {
        None  = 0,
        Quiet = 1 // --quiet
    }
}
