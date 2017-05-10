using System;

using Carbon.Data;
using Carbon.Data.Sequences;

namespace Carbon.Platform.VersionControl
{
    public class RepositoryDb
    {
        public RepositoryDb(IDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            
            Repositories        = new Dataset<RepositoryInfo,   long>(context, GetSequence("repositories"));
            Commits             = new Dataset<RepositoryCommit, long>(context);
            RepositoryFiles     = new Dataset<RepositoryFile,   (long, string, string)>(context);
            RepositoryBranches  = new Dataset<RepositoryBranch, (long, string)>(context);
        }

        public IDbContext Context { get; }

        public DbSequence GetSequence(string name) => new DbSequence(name, Context);

        public Dataset<RepositoryCommit, long>                  Commits { get; }
        public Dataset<RepositoryInfo, long>                    Repositories { get; }
        public Dataset<RepositoryFile, (long, string, string)>  RepositoryFiles { get; }
        public Dataset<RepositoryBranch, (long, string)>        RepositoryBranches { get; }
    }
}

// TODO: RepositoryObjects