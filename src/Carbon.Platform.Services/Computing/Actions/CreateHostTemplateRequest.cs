using System.ComponentModel.DataAnnotations;

using Carbon.Json;

namespace Carbon.Platform.Computing
{
    public class CreateHostTemplateRequest
    {
        public CreateHostTemplateRequest( 
            long ownerId,
            string name,
            long imageId,
            long machineTypeId,
            int locationId,
            string startupScript = null,
            JsonObject properties = null)
        {
            Ensure.IsValidId(ownerId,       nameof(ownerId));
            Ensure.NotNullOrEmpty(name,     nameof(name));
            Ensure.IsValidId(imageId,       nameof(imageId));
            Ensure.IsValidId(machineTypeId, nameof(machineTypeId));

            OwnerId       = ownerId;
            Name          = name;
            LocationId    = locationId; // may be universal (0)
            ImageId       = imageId;
            MachineTypeId = machineTypeId;
            StartupScript = startupScript;
            Properties    = properties;
        }

        public long OwnerId { get; }

        [Required]
        [StringLength(63)]
        public string Name { get; }
        
        public long ImageId { get; }

        public long MachineTypeId { get; }

        public int LocationId { get; }

        [StringLength(4000)]
        public string StartupScript { get; }

        public JsonObject Properties { get; }
    }    
}