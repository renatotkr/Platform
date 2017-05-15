using System.ComponentModel.DataAnnotations;
using Carbon.Platform.Apps;
using Carbon.Versioning;

namespace Carbon.Platform.Services
{
    public class CreateAppReleaseRequest
    {
        // [Range(1, 2_199_023_255_552)]
        public IApp App { get; set; }

        public SemanticVersion Version { get; set; }

        [Required]
        [MaxLength(32)]
        public byte[] Sha256 { get; set; }

        [Range(1, 2_199_023_255_552)]
        public long CreatorId { get; set; }
    }
}
