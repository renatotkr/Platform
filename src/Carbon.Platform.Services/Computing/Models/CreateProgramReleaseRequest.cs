using System;
using System.ComponentModel.DataAnnotations;

using Carbon.Json;
using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    public class CreateProgramReleaseRequest
    {
        public CreateProgramReleaseRequest() { }

        public CreateProgramReleaseRequest(
            IProgram program, 
            SemanticVersion version,
            JsonObject properties,
            long creatorId)
        {
            #region Preconditions

            Validate.Id(creatorId, nameof(creatorId));

            #endregion

            Program    = program ?? throw new ArgumentNullException(nameof(program));
            Version    = version;
            Properties = properties ?? throw new ArgumentNullException(nameof(properties));
            CreatorId  = creatorId;
        }

        [Required]
        public IProgram Program { get; set; }

        public SemanticVersion Version { get; set; }
        
        public JsonObject Properties { get; set; }

        [Range(1, 2_199_023_255_552)]
        public long CreatorId { get; set; }
    }
}