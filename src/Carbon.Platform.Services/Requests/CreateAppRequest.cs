using System.ComponentModel.DataAnnotations;

namespace Carbon.Platform.Apps
{
    public class CreateAppRequest
    {
        [Required]
        public string Name { get; set; }
        
        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; set; }
    }
}