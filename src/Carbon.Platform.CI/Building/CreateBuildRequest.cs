using System;

using Carbon.Platform.Storage;

namespace Carbon.Platform.CI
{
    // Should be startBuildRequest, but avoiding conflict with codebuild...

    public class CreateBuildRequest
    {
        public CreateBuildRequest() { }

        public CreateBuildRequest(
            RepositoryInfo repository,
            IRepositoryCommit commit, 
            long creatorId)
        {
            Source    = new RepositorySource(repository, commit);
            CreatorId = creatorId;
        }
        
        public RepositorySource Source { get; set; }

        public BuildOutput Output { get; set; }

        public long CreatorId { get; set; }
    }

    public class BuildOutput
    {
        public string BucketName { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        // Encoding
    }

    public class RepositorySource
    {
        public RepositorySource(RepositoryInfo repository, IRepositoryCommit commit)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            Commit     = commit ?? throw new ArgumentNullException(nameof(commit));
        }

        public RepositoryInfo Repository { get; }

        // presolved commit
        public IRepositoryCommit Commit { get; }
    }
}