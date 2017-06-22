using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Logging;
using Carbon.Platform.Resources;
using Carbon.Platform.Sequences;
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
            this.db             = db             ?? throw new ArgumentNullException(nameof(db));
            this.programService = programService ?? throw new ArgumentNullException(nameof(programService));
        }

        public async Task<ProgramRelease> CreateAsync(CreateProgramReleaseRequest request)
        {
            #region Preconditions

            Validate.Object(request, nameof(request));
            
            if (await ExistsAsync(request.Program.Id, request.Version).ConfigureAwait(false))
            {
                throw new Exception($"{request.Program.Id}@{request.Version} already exists");
            }

            #endregion

            var program = request.Program;

            var release = new ProgramRelease(
                id         : await ProgramReleaseId.NextAsync(db.Context, program.Id),
                program    : request.Program,
                version    : request.Version,
                properties : request.Properties,
                creatorId  : request.CreatorId
            );

            await db.ProgramReleases.InsertAsync(release).ConfigureAwait(false);

            if (request.Version > program.Version)
            {
                await db.Programs.PatchAsync(release.ProgramId, changes: new[] {
                    Change.Replace("version", release.Version)
                }).ConfigureAwait(false);
            }

            #region Logging

            await db.Activities.InsertAsync(new Activity("publish", program as IResource)).ConfigureAwait(false);

            #endregion

            return release;
        }

        public Task<ProgramRelease> GetAsync(long programId, SemanticVersion version)
        {
            return db.ProgramReleases.QueryFirstOrDefaultAsync(
                And(Eq("programId", programId), Eq("version", version))
            );
        }

        public async Task<bool> ExistsAsync(long programId, SemanticVersion version)
        {
            return await db.ProgramReleases.CountAsync(
                And(Eq("programId", programId), Eq("version", version))
            ) > 0;
        }

        public Task<IReadOnlyList<ProgramRelease>> ListAsync(long programId)
        {
            return db.ProgramReleases.QueryAsync(
                And(Eq("programId", programId), IsNull("deleted")),
                Order.Descending("version")
            );
        }
    }

    internal class ProgramReleaseId
    {
        static readonly string sql = SqlHelper.GetCurrentValueAndIncrement<Program>("releaseCount");

        public static async Task<long> NextAsync(IDbContext context, long programId)
        {
            using (var connection = context.GetConnection())
            {
                var currentReleaseCount = await connection.ExecuteScalarAsync<int>(sql, new { id = programId }).ConfigureAwait(false);

                return ScopedId.Create(programId, currentReleaseCount + 1);
            }
        }
    }
}