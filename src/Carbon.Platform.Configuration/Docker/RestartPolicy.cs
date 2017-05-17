namespace Carbon.Platform.Configuration.Docker
{
    public struct RestartPolicy
    {
        public static readonly RestartPolicy None = new RestartPolicy(); // default

        public static readonly RestartPolicy Always = new RestartPolicy();

        public int? MaxFailures { get; set; }

        public static RestartPolicy OnFailure(int maxRetries)
        {
            return new RestartPolicy { MaxFailures = maxRetries };
        }
    }
}
