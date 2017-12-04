using System;
using Carbon.Platform.Computing;
using Carbon.Json;

namespace Carbon.Platform.Environments
{
    public readonly struct CreateEnvironmentProgramRequest
    {
        public CreateEnvironmentProgramRequest(
            IEnvironment environment,
            IProgram program, 
            JsonObject configuration,
            long? userId = null)
        {
            Environment   = environment   ?? throw new ArgumentNullException(nameof(environment));
            Program       = program       ?? throw new ArgumentNullException(nameof(program));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            UserId        = userId;
        }

        public IEnvironment Environment { get; }

        public IProgram Program { get; }

        public JsonObject Configuration { get; }

        public long? UserId { get; }
    }
}
