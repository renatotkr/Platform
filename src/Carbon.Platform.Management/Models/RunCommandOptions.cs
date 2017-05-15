namespace Carbon.Platform.Management
{
    public class RunCommandOptions
    {
        private static readonly RunCommandOptions Default = new RunCommandOptions();

        public RunCommandOptions(string maxErrors = null, string maxConcurrency = null)
        {
            MaxErrors      = maxErrors;
            MaxConcurrency = maxConcurrency;
        }

        public string MaxErrors { get; }

        public string MaxConcurrency { get; }
    }
}