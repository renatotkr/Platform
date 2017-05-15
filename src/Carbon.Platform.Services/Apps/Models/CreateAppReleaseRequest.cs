using System.ComponentModel.DataAnnotations;

using Carbon.Versioning;

namespace Carbon.Platform.Apps
{
    public class CreateAppReleaseRequest
    {
        public CreateAppReleaseRequest() { }

        public CreateAppReleaseRequest(
            long appId, 
            SemanticVersion version,
            byte[] sha256, 
            long creatorId)
        {
            AppId       = appId;
            Version     = version;
            Sha256      = sha256;
            CreatorId   = creatorId;
        }

        [Range(1, 2_199_023_255_552)]
        public long AppId { get; set; }

        public SemanticVersion Version { get; set; }

        [Required]
        [MaxLength(32)]
        public byte[] Sha256 { get; set; }

        [Range(1, 2_199_023_255_552)]
        public long CreatorId { get; set; }
    }
}