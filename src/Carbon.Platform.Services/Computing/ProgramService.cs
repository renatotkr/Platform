using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;
using Carbon.Json;

namespace Carbon.Platform.Computing
{
    using static Expression;

    // Programs can be Apps, Sites, Services, or Tasks

    public class ProgramService : IProgramService
    {
        private readonly PlatformDb db;
        private readonly IEnvironmentService envService;

        public ProgramService(PlatformDb db, IEnvironmentService envService)
        {
            this.db         = db         ?? throw new ArgumentNullException(nameof(db));
            this.envService = envService ?? throw new ArgumentNullException(nameof(envService));
        }

        public async Task<ProgramInfo> GetAsync(long id)
        {
            return await db.Programs.FindAsync(id).ConfigureAwait(false)
                ?? throw ResourceError.NotFound(ResourceTypes.Program, id);
        }

        public Task<ProgramInfo> FindAsync(string slug)
        {
            if (long.TryParse(slug, out var id))
            {
                return db.Programs.FindAsync(id);
            }
            else
            {
                return db.Programs.QueryFirstOrDefaultAsync(Eq("slug", slug));
            }
        }

        public Task<IReadOnlyList<ProgramInfo>> ListAsync(long ownerId)
        {
            return db.Programs.QueryAsync(
                And(Eq("ownerId", ownerId), IsNull("deleted")),
                Order.Ascending("name")
             );
        }

        // TODO: Create the environments
        public async Task<ProgramInfo> CreateAsync(CreateProgramRequest request)
        {
            #region Preconditions

            Validate.Object(request, nameof(request));

            if (ProgramName.Validate(request.Name) == false)
                throw new ArgumentException($"Invalid name '{request.Name}", nameof(request.Name));

            #endregion

            // this sequence incriments by 4, reserving 4 consecutive ids
            var id = db.Programs.Sequence.Next(); 

            var program = new ProgramInfo(
                id         : id,
                name       : request.Name,
                slug       : request.Slug,
                properties : new JsonObject(),
                runtime    : request.Runtime,
                addresses  : request.Addresses,
                ownerId    : request.OwnerId
            );

            // Each app is given 4 environments (using the consecutive ids reserved above): 
            // Production, Staging, Intergration, and Development

            var environments = new[] {
                GetConfiguredEnvironment(program, EnvironmentType.Production),   // 1
                GetConfiguredEnvironment(program, EnvironmentType.Staging),      // 2
                GetConfiguredEnvironment(program, EnvironmentType.Intergration), // 3
                GetConfiguredEnvironment(program, EnvironmentType.Development),  // 4
            };
            
            await db.Programs.InsertAsync(program).ConfigureAwait(false);
            
            await db.Environments.InsertAsync(environments).ConfigureAwait(false);

            return program;
        }
       
        #region Environments

        public Task<EnvironmentInfo> GetEnvironmentAsync(long programId, EnvironmentType type)
        {
            return envService.GetAsync(programId, type);
        }

        public Task<IReadOnlyList<EnvironmentInfo>> GetEnvironmentsAsync(IProgram program)
        {            
            // + 0 production
            // + 1 staging
            // + 2 intergration
            // + 3 development

            return db.Environments.QueryAsync(Between("id", program.Id, program.Id + 3));
        }

        #endregion

        #region Helpers

        private async Task<EnvironmentInfo> CreateEnvironmentAsync(
            ProgramInfo program, EnvironmentType type)
        {
            var env = GetConfiguredEnvironment(program, type);

            await db.Environments.InsertAsync(env).ConfigureAwait(false);

            return env;
        }

        private EnvironmentInfo GetConfiguredEnvironment(
            ProgramInfo program, 
            EnvironmentType type)
        {
            // Production   = programId
            // Staging      = programId + 1
            // Intergration = programId + 2
            // Development  = programId + 3

            var envIdOffset = ((int)type) - 1;

            var name = program.Name;

            // accelerator/production

            if (type != EnvironmentType.Production)
            {
                name += "/" + type.ToString().ToLower();
            }
            return new EnvironmentInfo(
                id      : program.Id + envIdOffset,
                name    : name,
                ownerId : program.OwnerId
            );
        }

        #endregion
    }
}