// using System.ComponentModel.DataAnnotations;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class RegisterImageRequest
    {
        public RegisterImageRequest() { }

        public RegisterImageRequest(
            string name,
            ManagedResource resource,
            ImageType type = ImageType.Machine)
        {
            Name     = name;
            Resource = resource;
            Type     = type;
        }
        
        // [Required, StringLenth(3, 128)]
        public string Name { get; set; }

        public long OwnerId { get; set; }

        public ImageType Type { get; set; } = ImageType.Machine;

        public ManagedResource Resource { get; set; }
    }    
}