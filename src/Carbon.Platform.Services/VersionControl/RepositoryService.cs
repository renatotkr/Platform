using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Resources;

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

        public async Task<RepositoryInfo> GetAsync(long id)
        {
            var repository = await db.Repositories.FindAsync(id).ConfigureAwait(false);

            if (repository == null) throw new Exception($"repository#{id} not found");

            return repository;
        }

        public async Task<RepositoryInfo> GetAsync(string name)
        {
            var repository = await db.Repositories.QueryFirstOrDefaultAsync(Eq("name", name)).ConfigureAwait(false);

            if (repository == null) throw new Exception($"repository named '{name}' does not exist");

            return repository;
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

        public async Task<IRepositoryCommit> CreateCommitAsync(CreateCommitRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            RepositoryCommit commit;

            using (var connection = db.Context.GetConnection())
            using (var ts = connection.BeginTransaction())
            {
                var commitCount = connection.ExecuteScalar<int>(
                    @"SELECT `commitCount` FROM `Repositories` WHERE `id` = @id", new {
                        id = request.RepositoryId
                    }, ts);

                commit = new RepositoryCommit(
                    id      : ScopedId.Create(request.RepositoryId, commitCount),
                    sha1    : request.Sha1,
                    message : request.Message
                );

                // TODO: Do this inside the same transaction
                await db.Commits.InsertAsync(commit);

                connection.ExecuteScalar(
                    @"UPDATE `Repositories` 
                      SET `commitCount` = `commitCount` + 1 
                      WHERE `id` = @id", new {
                    id = request.RepositoryId    
                }, ts);

                ts.Commit();
            }

            return commit;
        }

        public async Task CreateBranchAsync(CreateBranchRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var branch = new RepositoryBranch(request.RepositoryId, request.Name, request.CreatorId);

            await db.RepositoryBranches.InsertAsync(branch).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<RepositoryBranch>> GetBranchesAsync(long repositoryId)
        {
            var result = await db.RepositoryBranches.QueryAsync(Eq("repositoryId", repositoryId)).ConfigureAwait(false);

            return result;
        }

        public Task<RepositoryBranch> GetBranchAsync(long repositoryId, string name)
        {
            return db.RepositoryBranches.FindAsync((repositoryId, name));
        }

        public Task<IReadOnlyList<RepositoryFile>> GetFilesAsync(long repositoryId, string branchName)
        {
            return db.RepositoryFiles.QueryAsync(
                Conjunction(Eq("repositoryId", repositoryId), Eq("branchName", branchName), IsNull("deleted"))
            );
        }

        public async Task<RepositoryFile> PutFileAsync(CreateFileRequest request)
        {
            var file = new RepositoryFile(
                repositoryId : request.RepositoryId, 
                branchName   : request.BranchName,
                path         : request.Path,
                type         : FileType.Blob) {
                CreatorId = request.CreatorId,
                Size      = request.Size,
                Sha256    = request.Sha256
            };

            using (var connection = db.Context.GetConnection())
            {
                await connection.ExecuteAsync(
                    @"INSERT INTO `RepositoryFiles` (`repositoryId`, `branchName`, `path`, `type`, `size`, `sha256`, `creatorId`)
                      VALUES (@repositoryId, @branchName, @path, @type, @size, @sha256, @creatorId)
                      ON DUPLICATE KEY UPDATE `size` = @size, `sha256` = @sha256;", file
                ).ConfigureAwait(false);

            }

            return file;
        }
    }
}