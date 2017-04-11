namespace Carbon.Platform.CI
{
    public struct DeployResult
    {
        public DeployResult(bool succeeded, string message = null)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public bool Succeeded { get; }

        public string Message { get; }
    }
}