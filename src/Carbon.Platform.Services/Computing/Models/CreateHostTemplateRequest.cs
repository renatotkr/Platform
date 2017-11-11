using System;
using System.ComponentModel.DataAnnotations;

using Carbon.Json;

namespace Carbon.Platform.Computing
{
    public class CreateHostTemplateRequest
    {
        public CreateHostTemplateRequest(
            string name,
            long ownerId,
            IImage image,
            IMachineType machineType,
            int locationId,
            string startupScript = null,
            JsonObject properties = null)
        {
            Validate.NotNullOrEmpty(name, nameof(name));

            Name          = name  ;
            OwnerId       = ownerId;
            LocationId    = locationId;
            Image         = image       ?? throw new ArgumentNullException(nameof(image));
            MachineType   = machineType ?? throw new ArgumentNullException(nameof(machineType));
            StartupScript = startupScript;
            Properties    = properties;
        }
        
        [Required]
        public string Name { get; }

        public long OwnerId { get; }

        [Required]
        public IImage Image { get; }

        [Required]
        public IMachineType MachineType { get; }

        public int LocationId { get; }

        public JsonObject Properties { get; }

        public string StartupScript { get; }
    }    
}