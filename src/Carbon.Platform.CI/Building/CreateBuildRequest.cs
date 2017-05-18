using System;

using Carbon.Platform.Storage;

namespace Carbon.Platform.CI
{
    // Should be startBuildRequest... but conflicts with codebuild...

    public class CreateBuildRequest
    {
        public RepositorySource Source { get; set; }

        public long CreatorId { get; set; }
    }
    
    public class RepositorySource
    {
        public RepositorySource(IRepositoryCommit commit)
        {
            Commit = commit ?? throw new ArgumentNullException(nameof(commit));
        }

        public long RepositoryId => Commit.RepositoryId;

        // presolved commit
        public IRepositoryCommit Commit { get; }
    }
}