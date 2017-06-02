using System;
using Carbon.Platform.Storage;

namespace Carbon.CI
{
    // Should be startBuildRequest, but avoiding conflict with codebuild...

    public class StartBuildRequest
    {
        public StartBuildRequest(
            IProject project,
            IRepositoryCommit commit,
            long initiatorId)
        {
            Project     = project     ?? throw new ArgumentNullException(nameof(project));
            Commit      = commit      ?? throw new ArgumentNullException(nameof(commit));
            InitiatorId = initiatorId;
        }
        
        public IProject Project { get; }

        public IRepositoryCommit Commit { get; }

        public BuildOutput Output { get; set; }

        public long InitiatorId { get; }
    }

    public class BuildOutput
    {
        public BuildOutput(string bucketName, string path, string name = null)
        {
            BucketName = bucketName;
            Path       = path;
            Name       = name;
        }

        public string BucketName { get; }

        public string Path { get; }

        public string Name { get; }

        // Encoding
    }
}