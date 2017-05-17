using System.ComponentModel.DataAnnotations;

using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    public class CreateProgramReleaseRequest
    {
        public CreateProgramReleaseRequest() { }

        public CreateProgramReleaseRequest(
            long programId, 
            SemanticVersion version,
            byte[] sha256, 
            long creatorId)
        {
            ProgramId   = programId;
            Version     = version;
            Sha256      = sha256;
            CreatorId   = creatorId;
        }

        [Range(1, 2_199_023_255_552)]
        public long ProgramId { get; set; }

        public SemanticVersion Version { get; set; }

        [Required]
        [MaxLength(32)]
        public byte[] Sha256 { get; set; }

        [Range(1, 2_199_023_255_552)]
        public long CreatorId { get; set; }
    }
}