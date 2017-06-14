using System;
using System.ComponentModel.DataAnnotations;

using Carbon.CI;
using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    public class CreateProgramReleaseRequest
    {
        public CreateProgramReleaseRequest() { }

        public CreateProgramReleaseRequest(
            Program program, 
            SemanticVersion version,
            IPackageInfo package,
            long creatorId)
        {
            Program   = program ?? throw new ArgumentNullException(nameof(program));
            Version   = version;
            Package   = package ?? throw new ArgumentNullException(nameof(package));
            CreatorId = creatorId;
        }

        [Required]
        public Program Program { get; set; }

        public SemanticVersion Version { get; set; }

        [Required]
        public IPackageInfo Package { get; set; }

        [Range(1, 2_199_023_255_552)]
        public long CreatorId { get; set; }
    }
}