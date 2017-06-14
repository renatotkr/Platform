using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Logging;
using Carbon.Versioning;

using Dapper;

namespace Carbon.Platform.Computing
{
    using static Expression;

    public class ProgramReleaseService : IProgramReleaseService
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

            var program = request.Program;

            var release = new ProgramRelease(
                id         : await GetNextId(program.Id),
                program    : request.Program,
                version    : request.Version,
                runtime    : program.Runtime,
                package    : request.Package,
                properties : program.Properties,
                creatorId  : request.CreatorId
            );

            await db.ProgramReleases.InsertAsync(release).ConfigureAwait(false);

            if (request.Version > program.Version)
            {
                await db.Programs.PatchAsync(
                    key: release.ProgramId,
                    changes: new[]  {
                        Change.Replace("version", release.Version)
                    }
                ).ConfigureAwait(false);
            }

            #region Logging

            await db.Activities.InsertAsync(new Activity("publish", program)).ConfigureAwait(false);

            #endregion

            return release;
        }

        public Task<ProgramRelease> GetAsync(long programId, SemanticVersion version)
        {
            return db.ProgramReleases.QueryFirstOrDefaultAsync(
                And(Eq("programId", programId), Eq("version", version))
            );
        }

        public Task<IReadOnlyList<ProgramRelease>> ListAsync(long appId)
        {
            return db.ProgramReleases.QueryAsync(Eq("programId", appId), Order.Descending("version"));
        }

        #region Helper

        private async Task<long> GetNextId(long programId)
        {
            using (var connection = db.Context.GetConnection())
            {
                return (await connection.ExecuteScalarAsync<int>(
                    @"SELECT `releaseCount` FROM `Programs` WHERE id = @id FOR UPDATE;
                      UPDATE `Programs`
                      SET `releaseCount` = `releaseCount` + 1
                      WHERE id = @id", new { id = programId }).ConfigureAwait(false)) + 1;
            }
        }

        #endregion
    }
}