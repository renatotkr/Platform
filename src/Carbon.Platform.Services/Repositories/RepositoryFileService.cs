using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;

using Dapper;

namespace Carbon.Platform.Storage
{
    using static Expression;

    public class RepositoryFileService : IRepositoryFileService
    {
        private readonly RepositoryDb db;

        public RepositoryFileService(RepositoryDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<RepositoryFile>> ListAsync(long branchId)
        {
            return db.RepositoryFiles.QueryAsync(
                And(Eq("branchId", branchId), IsNull("deleted")
            ));
        }
        
        public Task<RepositoryFile> FindAsync(long branchId, string path)
        {
            return db.RepositoryFiles.FindAsync((branchId, path));
        }

        public Task<IReadOnlyList<RepositoryFile>> ListChangedSince(long branchId, DateTime modified)
        {
            return db.RepositoryFiles.QueryAsync(
                Conjunction(
                    Eq("branchId", branchId),
                    Or(Gte("modified", modified), Gte("deleted", modified))
                )
            );
        }

        public Task<IReadOnlyList<RepositoryFile>> ListGetsSinceAsync(long branchId)
        {
            return db.RepositoryFiles.QueryAsync(
                Conjunction(
                    Eq("branchId", branchId),
                    IsNull("deleted")
                )
            );
        }

        public async Task DeleteAsync(DeleteFileRequest request)
        {
            await db.RepositoryFiles.PatchAsync(
                key : (request.BranchId, request.Path),
                changes: new[] {
                    Change.Replace("deleted", Func("NOW"))
                }
            );
        }

        public async Task<RepositoryFile> PutAsync(CreateFileRequest request)
        {
            #region Preconditions

            Validate.Object(request, nameof(request));

            #endregion

            var file = new RepositoryFile(
                branchId  : request.BranchId,
                type      : FileType.Blob,
                path      : request.Path,
                size      : request.Size,
                sha256    : request.Sha256,
                creatorId : request.CreatorId
            );

            using (var connection = db.Context.GetConnection())
            {
                await connection.ExecuteAsync(
                    @"INSERT INTO `RepositoryFiles` (`branchId`, `path`, `version`, `type`, `size`, `sha256`, `creatorId`)
                      VALUES (@branchId, @path, @version, @type, @size, @sha256, @creatorId)
                      ON DUPLICATE KEY UPDATE 
                        `size` = @size,
                        `sha256` = @sha256,
                        `deleted` = NULL,
                        `version` = `version` + 1;", file
                ).ConfigureAwait(false);
            }

            return file;
        }
    }
}