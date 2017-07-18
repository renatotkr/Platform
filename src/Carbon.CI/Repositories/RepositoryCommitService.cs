using System;
using System.Threading.Tasks;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

namespace Carbon.CI
{
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
        
        public async Task<RepositoryCommit> CreateAsync(CreateCommitRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var commit = await FindAsync(request.RepositoryId, request.Sha1);

            if (commit == null)
            { 
                commit = new RepositoryCommit(
                    id           : await CommitId.NextAsync(db.Context, request.RepositoryId),
                    repositoryId : request.RepositoryId,
                    sha1         : request.Sha1,
                    message      : request.Message
                );

                await db.RepositoryCommits.InsertAsync(commit);
            }

            return commit;
        }
    }
}