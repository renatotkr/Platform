using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Environments;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    using static Expression;

    public class ProgramService : IProgramService
    {
        private readonly PlatformDb db;

        public ProgramService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<ProgramInfo> GetAsync(long id)
        {
            return await db.Programs.FindAsync(id)
                ?? throw ResourceError.NotFound(ResourceTypes.Program, id);
        }

        public Task<ProgramInfo> FindAsync(string name)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            if (long.TryParse(name, out var id))
            {
                return db.Programs.FindAsync(id);
            }
            else
            {
                return db.Programs.QueryFirstOrDefaultAsync(Eq("slug", name));
            }
        }

        public Task<IReadOnlyList<ProgramInfo>> ListAsync(long ownerId)
        {
            return db.Programs.QueryAsync(
                And(Eq("ownerId", ownerId), IsNull("deleted")),
                Order.Ascending("name")
             );
        }

        public Task<IReadOnlyList<ProgramInfo>> ListAsync(long ownerId, ProgramType type)
        {
            return db.Programs.QueryAsync(
                Conjunction(Eq("ownerId", ownerId), Eq("type", type), IsNull("deleted")),
                Order.Ascending("name")
             );
        }

        public async Task<IReadOnlyList<ProgramInfo>> ListAsync(IEnvironment environment)
        {
            #region Preconditions

            if (environment == null)
                throw new ArgumentNullException(nameof(environment));

            #endregion

            // TODO: Do a left JOIN on on programs

            var records = await db.EnvironmentPrograms.QueryAsync(
                And(Eq("environmentId", environment.Id), IsNull("deleted"))
            );

            var programs = new List<ProgramInfo>(records.Count);

            foreach (var record in records)
            {
                var program = await GetAsync(record.ProgramId);

                programs.Add(new ProgramInfo(
                    id           : program.Id, 
                    name         : program.Name,
                    slug         : program.Slug, 
                    ownerId      : program.OwnerId,
                    version      : record.ProgramVersion,
                    properties   : record.Configuration,
                    type         : program.Type,
                    repositoryId : program.RepositoryId,
                    parentId     : program.ParentId
                ));
            }
            
            return programs;
        }

        public async Task<IReadOnlyList<ProgramInfo>> ListAsync(IHost host)
        {
            #region Preconditions

            if (host == null)
                throw new ArgumentNullException(nameof(host));

            #endregion

            // TODO: Do a left JOIN on on programs

            var records = await db.HostPrograms.QueryAsync(
                And(Eq("hostId", host.Id), IsNull("deleted"))
            );

            var programs = new List<ProgramInfo>(records.Count);

            foreach (var record in records)
            {
                var program = await GetAsync(record.ProgramId);

                programs.Add(new ProgramInfo(
                    id           : program.Id, 
                    name         : program.Name,
                    slug         : program.Slug, 
                    ownerId      : program.OwnerId,
                    version      : record.ProgramVersion,
                    runtime      : record.Runtime,
                    properties   : record.Properties,
                    type         : program.Type,
                    repositoryId : program.RepositoryId,
                    parentId     : program.ParentId
                ));
            }
            
            return programs;
        }

        public async Task<ProgramInfo> CreateAsync(CreateProgramRequest request)
        {
            #region Preconditions

            Validate.Object(request, nameof(request));

            if (request.Type != ProgramType.Site && ProgramName.Validate(request.Name) == false)
                throw new ArgumentException($"Not a valid program name '{request.Name}", nameof(request.Name));

            #endregion

            var program = new ProgramInfo(
                id           : await db.Programs.Sequence.NextAsync(),
                type         : request.Type,
                name         : request.Name,
                slug         : request.Slug,
                version      : request.Version,
                properties   : request.Properties,
                runtime      : request.Runtime,
                addresses    : request.Addresses,
                repositoryId : request.RepositoryId,
                ownerId      : request.OwnerId,
                parentId     : request.ParentId
            );
            
            await db.Programs.InsertAsync(program);
            
            return program;
        }

        public async Task<bool> DeleteAsync(IProgram program)
        {
            #region Preconditions

            if (program == null)
                throw new ArgumentNullException(nameof(program));

            #endregion

            return await db.Programs.PatchAsync(program.Id, new[] {
                Change.Replace("deleted", Func("NOW"))
            }, condition: IsNull("deleted")) > 0;
        }
    }
}

// Programs can be Apps, Sites, Services, or Tasks
