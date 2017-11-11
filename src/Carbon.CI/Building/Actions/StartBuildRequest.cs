using System;

namespace Carbon.CI
{
    public class StartBuildRequest
    {
        public StartBuildRequest(IProject project, IRepositoryCommit commit)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
            Commit = commit ?? throw new ArgumentNullException(nameof(commit));
        }

        public IProject Project { get; }

        public IRepositoryCommit Commit { get; }

        public BuildOutput Output { get; set; }
    }

    public class BuildOutput
    {
        public BuildOutput(string bucketName, string path, string name = null)
        {
            BucketName = bucketName;
            Path = path;
            Name = name;
        }

        public string BucketName { get; }

        public string Path { get; }

        public string Name { get; }

        // Encoding
    }
}