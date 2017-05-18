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
            string name,
            IMachineType machineType,
            IMachineImage machineImage,
            JsonObject details,
            long ownerId,
            ManagedResource resource)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentNullException("Must be > 0", nameof(id));

            if (machineType == null)
                throw new ArgumentNullException(nameof(machineType));

            if (machineImage == null)
                throw new ArgumentNullException(nameof(machineImage));

            #endregion

            Id             = id;
            Name           = name;
            MachineTypeId  = machineType.Id;
            MachineImageId = machineImage.Id;
            ProviderId     = resource.ProviderId;
            Details        = details ?? throw new ArgumentNullException(nameof(details));
            OwnerId        = ownerId;
            ResourceId     = resource.ResourceId;
        }

        [Member("id"), Key(sequenceName: "hostTemplateId")]
        public long Id { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("machineTypeId")]
        public long MachineTypeId { get; }

        [Member("machineImageId")]
        public long MachineImageId { get; }
        
        [Member("startupScript")]
        [StringLength(2000)]
        public string StartupScript { get; set; }
        
        [Member("details")]
        [StringLength(2000)]
        public JsonObject Details { get; }

        [Member("ownerId")]
        public long OwnerId { get; set; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }
        
        int IManagedResource.LocationId => 0;

        ResourceType IResource.ResourceType => ResourceTypes.HostTemplate;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }
        
        [IgnoreDataMember]
        [Member("deleted"), Timestamp]
        public DateTime? Deleted { get; }
        
        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}