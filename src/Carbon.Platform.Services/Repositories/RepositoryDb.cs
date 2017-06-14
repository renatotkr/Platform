using System;

using Carbon.Data;

namespace Carbon.Platform.Storage
{
    public class RepositoryDb
    {
        public RepositoryDb(IDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            
            Repositories       = new Dataset<RepositoryInfo,   long>(context);
            RepositoryBranches = new Dataset<RepositoryBranch, long>(context);
            RepositoryCommits  = new Dataset<RepositoryCommit, long>(context);
            RepositoryFiles    = new Dataset<RepositoryFile,   (long, string)>(context); // branchId + path
        }

        public IDbContext Context { get; }

        public Dataset<RepositoryInfo,   long> Repositories { get; }
        public Dataset<RepositoryBranch, long> RepositoryBranches { get; }
        public Dataset<RepositoryCommit, long> RepositoryCommits { get; }
        public Dataset<RepositoryFile,   (long branchId, string path)> RepositoryFiles { get; }
    }
}