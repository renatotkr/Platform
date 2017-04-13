using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("HostTemplates")]
    public class HostTemplate : IHostTemplate
    {
        public HostTemplate() { }
        
        public HostTemplate(
            long id, 
            IMachineType machineType,
            IMachineImage machineImage)
        {
            #region Preconditions

            if (machineType == null)
                throw new ArgumentNullException(nameof(machineType));

            if (machineImage == null)
                throw new ArgumentNullException(nameof(machineImage));

            #endregion

            Id             = id;
            MachineTypeId  = machineType.Id;
            MachineImageId = machineImage.Id;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("machineTypeId")]
        public long MachineTypeId { get; }

        [Member("machineImageId")]
        public long MachineImageId { get; }

        // Script at startup
        [Member("script")]
        public string Script { get; set; }

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        int IManagedResource.LocationId => 0;

        ResourceType IResource.ResourceType => ResourceType.HostTemplate;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted"), Timestamp]
        public DateTime? Deleted { get; }

        // Host templates are immutable
        // A new template must be made to make changes

        #endregion
    }
}
