using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Logs;
using Carbon.Versioning;

using Dapper;

namespace Carbon.Platform.Computing
{
    using static Expression;

    public class ProgramReleaseService
    {
        private readonly PlatformDb db;
        private readonly IProgramService programService;

        public ProgramReleaseService(PlatformDb db, IProgramService programService)
        {
            this.db             = db ?? throw new ArgumentNullException(nameof(db));
            this.programService = programService ?? throw new ArgumentNullException(nameof(programService));
        }

        public async Task<ProgramRelease> CreateAsync(CreateProgramReleaseRequest request)
        {
            #region Preconditions

            Validate.Object(request, nameof(request));

            #endregion

            // Fetch the program to ensure it exists...
            var program = await programService.GetAsync(request.ProgramId).ConfigureAwait(false);

            var release = new ProgramRelease(
                id        : await GetNextId(request.ProgramId), 
                programId : request.ProgramId,
                version   : request.Version, 
                sha256    : request.Sha256,
                creatorId : request.CreatorId
            );

            await db.ProgramReleases.InsertAsync(release).ConfigureAwait(false);

            #region Logging

            var activity = new Activity(ActivityType.Publish, program);

            await db.Activities.InsertAsync(activity).ConfigureAwait(false);

            #endregion

            return release;
        }

        public Task<ProgramRelease> GetReleaseAsync(long appId, SemanticVersion version)
        {
            return db.ProgramReleases.QueryFirstOrDefaultAsync(
                And(Eq("appId", appId), Eq("version", version))
            );
        }

        public Task<IReadOnlyList<ProgramRelease>> ListAsync(long appId)
        {
            return db.ProgramReleases.QueryAsync(Eq("programId", appId), Order.Descending("version"));
        }


        #region Helper

        private async Task<long> GetNextId(long appId)
        {
            using (var connection = db.Context.GetConnection())
            {
                return (await connection.ExecuteScalarAsync<int>(
                    @"SELECT `releaseCount` FROM `Programs` WHERE id = @id FOR UPDATE;
                      UPDATE `Programs`
                      SET `releaseCount` = `releaseCount` + 1
                      WHERE id = @id", new { id = appId }).ConfigureAwait(false)) + 1;
            }
        }

        #endregion
    }
}