using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Expressions;

namespace Carbon.Platform.Environments
{
    public class EnvironmentProgramService : IEnvironmentProgramService
    {
        private readonly PlatformDb db;

        public EnvironmentProgramService(PlatformDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IReadOnlyList<EnvironmentProgram>> ListAsync(IEnvironment environment)
        {
            Ensure.NotNull(environment, nameof(environment));

            return db.EnvironmentPrograms.QueryAsync(
                Expression.And(Expression.Eq("environmentId", environment.Id), Expression.IsNull("deleted"))
            );
        }

        public async Task<EnvironmentProgram> CreateAsync(CreateEnvironmentProgramRequest request)
        {
            Ensure.NotNull(request, nameof(request));

            var program = new EnvironmentProgram(
                environment   : request.Environment,
                program       : request.Program,
                configuration : request.Configuration,
                userId        : request.UserId
            );
            
            await db.EnvironmentPrograms.InsertAsync(program);

            return program;
        }
    }
}