using System;
using System.ComponentModel.DataAnnotations;
using Carbon.Platform.Storage;

namespace Carbon.Platform.CI
{
    // Should be startBuildRequest, but avoiding conflict with codebuild...

    public class CreateBuildRequest
    {
        public CreateBuildRequest() { }

        public CreateBuildRequest(
            IBuildProject project,
            IRepositoryCommit commit,
            long initiatorId)
        {
            Project     = project ?? throw new ArgumentNullException(nameof(project));
            Commit      = commit;
            InitiatorId = initiatorId;
        }
        
        public IBuildProject Project { get; set; }

        public IRepositoryCommit Commit { get; set; }

        public BuildOutput Output { get; set; }

        public long InitiatorId { get; set; }
    }

    public class BuildOutput
    {
        public BuildOutput() { }

        public BuildOutput(string bucketName, string path, string name = null)
        {
            BucketName = bucketName;
            Path       = path;
            Name       = name;
        }

        public string BucketName { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        // Encoding
    }
}