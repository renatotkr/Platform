using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Computing;
using Carbon.Versioning;

namespace Carbon.Platform.Environments
{
    [Dataset("EnvironmentPrograms")]
    public class EnvironmentProgram
    {
        public EnvironmentProgram() { }

        public EnvironmentProgram(
            IEnvironment environment,
            IProgram program, 
            JsonObject configuration,
            long? userId = null)
        {
            Validate.NotNull(environment, nameof(environment));
            Validate.NotNull(program,     nameof(program));

            EnvironmentId = environment.Id;
            ProgramId      = program.Id;
            ProgramVersion = program.Version;
            Configuration  = configuration ?? throw new ArgumentNullException(nameof(configuration));
            UserId         = userId;
        }

        [Member("environmentId"), Key]
        public long EnvironmentId { get; }

        [Member("programId"), Key]
        public long ProgramId { get; }

        [Member("programVersion")]
        public SemanticVersion ProgramVersion { get; }

        [Member("configuration")]
        [StringLength(1000)]
        public JsonObject Configuration { get; }
        
        [Member("userId")] // role / user / service / principal
        public long? UserId { get; }

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }

    public static class ProgramConfigurationProperties
    {
        public const string Runtime   = "runtime";
        public const string Addresses = "addressses";
    }
}
