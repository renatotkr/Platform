namespace Carbon.CI
{
    public static class PipelineStages
    {
        public const string Download = "download";  // Map an environment ref to a commit
        public const string Build    = "build";     // Build the commit
        public const string Test     = "test";      // run the build tests
        public const string Publish  = "publish";   // package the build & publish
        public const string Deploy   = "deploy";    // deploy the published build
    }
}