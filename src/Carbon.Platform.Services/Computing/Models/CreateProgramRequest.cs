using System;
using System.ComponentModel.DataAnnotations;

using Carbon.Json;

namespace Carbon.Platform.Computing
{
    public class CreateProgramRequest
    {
        public CreateProgramRequest() { }

        public CreateProgramRequest(
            string name, 
            long ownerId, 
            JsonObject properties,
            string[] urls = null)
        {
            #region Preconditions

            Validate.Id(ownerId, nameof(ownerId));

            #endregion

            Name       = name ?? throw new ArgumentNullException(nameof(name));
            OwnerId    = ownerId;
            Properties = properties;
            Urls       = urls;
        }

        [Required, StringLength(63)]
        public string Name { get; set; }
        
        public JsonObject Properties { get; set; }

        public string[] Urls { get; set; }

        [StringLength(63)]
        public string Slug { get; set; }
        
        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; set; }
    }
}