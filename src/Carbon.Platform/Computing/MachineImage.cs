using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("MachineImages", Schema = "Computing")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class MachineImage : IMachineImage
    {
        public MachineImage() { }

        public MachineImage(
            long id,
            MachineImageType type,
            string name,
            ManagedResource resource)
        {
            Id          = id;
            Type        = type;
            Name        = name ?? throw new ArgumentNullException(nameof(name));
            ResourceId  = resource.ResourceId;
            ProviderId  = resource.ProviderId;
            LocationId  = resource.LocationId;
        }

        [Member("id"), Key(sequenceName: "machineImageId")]
        public long Id { get; }
        
        [Member("type")]
        public MachineImageType Type { get; }
        
        [Member("name")]
        [StringLength(3, 128)]
        public string Name { get; }

        [Member("ownerId")]
        public long OwnerId { get; set; }

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.MachineImage;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        // Machine images are immutable 
        // They may only be marked as deleted

        #endregion
    }
}