using System;

/*
namespace Carbon.Platform.CI
{
    using static BuildStatus;

    public static class CodeBuildExtensions
    {
        public static BuildStatus GetStatus(this Amazon.CodeBuild.Build build)
        {
            switch (build.BuildStatus)
            {
                case "FAILED"      : return Failed;
                case "FAULT"       : return Failed;
                case "IN_PROGRESS" : return Building;
                case "STOPPED"     : return Pending;
                case "SUCCEEDED"   : return Completed;
                case "TIMED_OUT"   : return Failed;
            }

            throw new Exception("unexpected status:" + build.BuildStatus);
        }
    }
}
*/