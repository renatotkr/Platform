using System;

using codebuild = Amazon.CodeBuild;

namespace Carbon.CI
{
    using static BuildStatus;

    public static class CodeBuildExtensions
    {
        public static BuildStatus GetStatus(this codebuild::Build build)
        {
            switch (build.BuildStatus.ToUpper())
            {
                case "FAILED"      : return Failed;
                case "FAULT"       : return Failed;
                case "IN_PROGRESS" : return Pending;
                case "STOPPED"     : return Pending;
                case "SUCCEEDED"   : return Succeeded;
                case "TIMED_OUT"   : return Failed;
            }

            throw new Exception("unexpected status:" + build.BuildStatus);
        }
    }
}
