using System;
using System.Threading.Tasks;

using Carbon.Data.Expressions;

using Dapper;

namespace Carbon.Platform.VersionControl
{
    using static Expression;

    public class RepositoryService
    {
        private readonly RepositoryDb db;

        public RepositoryService(RepositoryDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task CreateAsync(string name, long ownerId, ManagedResource resource)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            var repository = new RepositoryInfo(
                id       : db.Context.GetNextId<RepositoryInfo>(),
                name     : name,
                ownerId  : ownerId,
                resource : resource
            );

            await db.Repositories.InsertAsync(repository).ConfigureAwait(false);
        }

        public async Task<RepositoryCommit> CreateCommitAsync(IRepository repository, byte[] sha1, string message)
        {
            #region Preconditions

            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            if (sha1 == null)
                throw new ArgumentNullException(nameof(sha1));

            #endregion

            RepositoryCommit commit;

            using (var connection = db.Context.GetConnection())
            using (var ts = connection.BeginTransaction())
            {
                var commitCount = connection.ExecuteScalar<int>(@"SELECT `commitCount` FROM `Repositories` WHERE `id` = @id", repository, ts);

                commit = new RepositoryCommit(
                    id      : ScopedId.Create(repository.Id, commitCount),
                    sha1    : sha1,
                    message : message
                );

                // TODO: Do this inside the same transaction
                await db.Commits.InsertAsync(commit);

                connection.ExecuteScalar("UPDATE `Repositories` SET `commitCount` = `commitCount` + 1 WHERE id = @id", repository, ts);

                ts.Commit();
            }

            return commit;
        }

        public async Task CreateBranchAsync(IRepository repository, string name)
        {
            var branch = new RepositoryBranch(repository.Id, name);

            await db.Branches.InsertAsync(branch).ConfigureAwait(false);
        }
    }
}