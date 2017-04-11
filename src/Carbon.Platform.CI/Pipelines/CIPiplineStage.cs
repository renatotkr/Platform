
namespace Carbon.Platform.CI
{
    public enum CIPiplineStage
    {
        Download    = 1, // Map an environment ref to a commit
        Build       = 2, // Build the commit
        Test        = 3, // run the build tests
        Publish     = 4, // package the build & publish
        Deploy      = 5, // deploy the published build
    }
}
