using System;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;

using Dapper;

namespace Carbon.Platform.Storage
{
    using static Expression;

    public class RepositoryCommitService : IRepositoryCommitService
    {
        private readonly RepositoryDb db;

        public RepositoryCommitService(RepositoryDb db)
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
            ).ConfigureAwait(false);
        }
        
        public async Task<RepositoryCommit> CreateAsync(CreateCommitRequest request)
        {
            #region Preconditions

            Validate.Object(request, nameof(request));

            #endregion

            var commit = await FindAsync(request.RepositoryId, request.Sha1).ConfigureAwait(false);

            if(commit == null)
            { 
                commit = new RepositoryCommit(
                    id           : await CommitId.NextAsync(db.Context, request.RepositoryId).ConfigureAwait(false),
                    repositoryId : request.RepositoryId,
                    sha1         : request.Sha1,
                    message      : request.Message
                );
            }

            await db.RepositoryCommits.InsertAsync(commit).ConfigureAwait(false);

            return commit;
        }
    }

    internal static class CommitId
    {
        public static async Task<long> NextAsync(
            IDbContext context,
            long repositoryId)
        {
            using (var connection = context.GetConnection())
            {
                var currentCommitCount = await connection.ExecuteScalarAsync<int>(
                  @"SELECT `commitCount` FROM `Repositories` WHERE id = @id FOR UPDATE;
                      UPDATE `Repositories`
                      SET `commitCount` = `commitCount` + 1
                      WHERE id = @id", new { id = repositoryId }).ConfigureAwait(false);

                return ScopedId.Create(repositoryId, currentCommitCount + 1);
            }
        }
    }
}