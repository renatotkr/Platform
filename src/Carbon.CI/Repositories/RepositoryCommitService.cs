using System;
using System.Threading.Tasks;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

namespace Carbon.CI
{
    using System.Collections.Generic;
    using Carbon.Data;
    using Carbon.Platform.Sequences;
    using static Expression;

    public class RepositoryCommitService : IRepositoryCommitService
    {
        private readonly CiadDb db;

        public RepositoryCommitService(CiadDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<RepositoryCommit> GetAsync(long id)
        {
            return await db.RepositoryCommits.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.RepositoryCommit, id);
        }

        public async Task<RepositoryCommit> FindAsync(long repositoryId, byte[] sha1)
        {
            return await db.RepositoryCommits.QueryFirstOrDefaultAsync(
                And(Eq("repositoryId", repositoryId), Eq("sha1", sha1))
            );
        }

        public Task<IReadOnlyList<RepositoryCommit>> ListAsync(IRepository repository)
        {
            var range = ScopedId.GetRange(repository.Id);

            return db.RepositoryCommits.QueryAsync(
               Between("id", range.Start, range.End),
               order: Order.Descending("id")
            );
        }

        public async Task<RepositoryCommit> CreateAsync(CreateCommitRequest request)
        {
            Ensure.NotNull(request, nameof(request));

            var commit = await FindAsync(request.RepositoryId, request.Sha1);

            if (commit == null)
            { 
                commit = new RepositoryCommit(
                    id           : await CommitId.NextAsync(db.Context, request.RepositoryId),
                    repositoryId : request.RepositoryId,
                    sha1         : request.Sha1,
                    authorId     : request.AuthorId,
                    message      : request.Message
                );

                await db.RepositoryCommits.InsertAsync(commit);
            }

            return commit;
        }
    }
}