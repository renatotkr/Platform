using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Sequences;

namespace Carbon.Rds.Services
{
    using static Expression;

    public class DatabaseBackupService : IDatabaseBackupService
    {
        private readonly RdsDb db;

        public DatabaseBackupService(RdsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<DatabaseBackup>> ListAsync(IDatabaseInfo database)
        {
            #region Preconditions

            if (database == null)
                throw new ArgumentNullException(nameof(database));

            #endregion

            var range = ScopedId.GetRange(database.Id);

            return db.DatabaseBackups.QueryAsync(
                And(Between("id", range.Start, range.End), IsNull("deleted"))
            );
        }

        public async Task<DatabaseBackup> StartAsync(StartDatabaseBackupRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var id = await DatabaseBackupId.NextAsync(db.Context, request.DatabaseId);

            var backup = new DatabaseBackup(
                id       : id, 
                bucketId : request.BucketId, 
                name     : request.Name
            );

            await db.DatabaseBackups.InsertAsync(backup);

            return backup;
        }

        public async Task CompleteAsync(CompleteDatabaseBackupRequest request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            await db.DatabaseBackups.PatchAsync(request.BackupId, new[] {
                Change.Replace("status",    request.Status),
                Change.Replace("message",   request.Message),
                Change.Replace("size",      request.Size),
                Change.Replace("sha256",    request.Sha256),
                Change.Replace("completed", Func("NOW"))
            });
        }
    }
}