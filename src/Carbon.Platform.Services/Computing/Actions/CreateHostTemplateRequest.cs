using System;
using System.ComponentModel.DataAnnotations;

using Carbon.Json;

namespace Carbon.Platform.Computing
{
    public class CreateHostTemplateRequest
    {
        public CreateHostTemplateRequest( 
            long ownerId,
            string name,
            IImage image,
            IMachineType machineType,
            int locationId,
            string startupScript = null,
            JsonObject properties = null)
        {
            Ensure.Id(ownerId);
            Ensure.NotNullOrEmpty(name, nameof(name));

            OwnerId       = ownerId;
            Name          = name;
            LocationId    = locationId;
            Image         = image       ?? throw new ArgumentNullException(nameof(image));
            MachineType   = machineType ?? throw new ArgumentNullException(nameof(machineType));
            StartupScript = startupScript;
            Properties    = properties;
        }

        public long OwnerId { get; }

        [Required]
        public string Name { get; }

        [Required]
        public IImage Image { get; }

        [Required]
        public IMachineType MachineType { get; }

        public int LocationId { get; }

        public JsonObject Properties { get; }

        public string StartupScript { get; }
    }    
}