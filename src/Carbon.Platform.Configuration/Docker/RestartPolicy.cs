namespace Carbon.Platform.Configuration.Docker
{
    public struct RestartPolicy
    {
        public static readonly RestartPolicy None = new RestartPolicy();
        public static readonly RestartPolicy Always = new RestartPolicy();

        public int? MaxFailures { get; set; }

        public static RestartPolicy OnFailure(int maxFailures)
        {
            return new RestartPolicy { MaxFailures = maxFailures };
        }
    }
}
