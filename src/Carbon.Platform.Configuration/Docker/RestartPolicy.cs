namespace Carbon.Platform.Configuration.Docker
{
    public readonly struct RestartPolicy
    {
        public static readonly RestartPolicy None = new RestartPolicy(); // default

        public static readonly RestartPolicy Always = new RestartPolicy();

        public RestartPolicy(int? maxFailures = null)
        {
            MaxFailures = maxFailures;
        }

        public int? MaxFailures { get; }

        public static RestartPolicy OnFailure(int maxRetries)
        {
            return new RestartPolicy(maxRetries);
        }
    }
}
