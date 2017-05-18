using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;
using Carbon.Platform.Services;

using Dapper;

namespace Carbon.Platform.VersionControl
{
    using static Expression;

    public class RepositoryService : IRepositoryService
    {
        private readonly RepositoryDb db;

        public RepositoryService(RepositoryDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<RepositoryInfo> GetAsync(long id)
        {
            return await db.Repositories.FindAsync(id).ConfigureAwait(false) ?? throw ResourceError.NotFound(ResourceTypes.Repository, id);
        }

        public async Task<RepositoryInfo> GetAsync(long ownerId, string name)
        {
            var repository = await db.Repositories.QueryFirstOrDefaultAsync(
                And(Eq("ownerId", ownerId), Eq("name", name))
            ).ConfigureAwait(false);

            if (repository == null)
            {
                throw ResourceError.NotFound(ResourceTypes.Repository, ownerId, name);
            }

            return repository;
        }

        public async Task<RepositoryInfo> GetAsync(ResourceProvider provider, string fullName)
        {
            var repository = await db.Repositories.QueryFirstOrDefaultAsync(
                And(Eq("providerId", provider.Id), Eq("fullName", fullName))
            ).ConfigureAwait(false);

            if (repository == null)
            {
                throw ResourceError.NotFound(provider, ResourceTypes.Repository, fullName);
            }

            return repository;
        }

        public async Task<RepositoryInfo> CreateAsync(CreateRepositoryRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var repository = new RepositoryInfo(
                id       : db.Repositories.Sequence.Next(),
                name     : request.Name,
                ownerId  : request.OwnerId,
                resource : request.Resource
            );

            await db.Repositories.InsertAsync(repository).ConfigureAwait(false);

            // Recreate the master branch
            var master = new RepositoryBranch(repository.Id, "master", request.OwnerId);

            await db.RepositoryBranches.InsertAsync(master).ConfigureAwait(false);

            return repository;
        }

        public async Task<IRepositoryCommit> GetCommitAsync(long repositoryId, byte[] sha)
        {
            var range = ScopedId.GetRange(repositoryId);

            // sha is assumed to be v1 for now
            // determine type on length once sha3 identifiers are introduced...

            return await db.Commits.QueryFirstOrDefaultAsync(
                And(
                    Eq("sha1", sha),
                    Between("id", range.Start, range.End)
                )
            ).ConfigureAwait(false);
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

        public async Task DeleteFileAsync(DeleteFileRequest request)
        {
            using (var connection = db.Context.GetConnection())
            {
                await connection.ExecuteAsync(
                    @"UPDATE `RepositoryFiles` 
                      SET `deleted` = NOW()  
                      WHERE `repositoryId` = @repositoryId
                        AND `branchName` = @branchName
                        AND `path` = @path;", request
                ).ConfigureAwait(false);
            }
        }

        public async Task<RepositoryFile> PutFileAsync(CreateFileRequest request)
        {
            var file = new RepositoryFile(
                repositoryId : request.RepositoryId, 
                branchName   : request.BranchName,
                type         : FileType.Blob,
                path         : request.Path,
                size         : request.Size,
                sha256       : request.Sha256,
                creatorId    : request.CreatorId
            );

            using (var connection = db.Context.GetConnection())
            {
                await connection.ExecuteAsync(
                    @"INSERT INTO `RepositoryFiles` (`repositoryId`, `branchName`, `path`, `type`, `size`, `sha256`, `creatorId`)
                      VALUES (@repositoryId, @branchName, @path, @type, @size, @sha256, @creatorId)
                      ON DUPLICATE KEY UPDATE `size` = @size, `sha256` = @sha256, `deleted` = NULL;", file
                ).ConfigureAwait(false);
            }

            return file;
        }
    }
}