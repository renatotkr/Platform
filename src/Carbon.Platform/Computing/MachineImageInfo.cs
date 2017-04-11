using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Computing
{
    [Dataset("MachineImages")]
    [DataIndex(IndexFlags.Unique, "providerId", "resourceId")]
    public class MachineImageInfo : IMachineImage
    {
        public MachineImageInfo() { }

        public MachineImageInfo(
            long id,
            MachineImageType type,
            string name, 
            string description,
            ManagedResource resource)
        {
            Id          = id;
            Type        = type;
            Name        = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            ResourceId  = resource.ResourceId;
            ProviderId  = resource.ProviderId;
            LocationId  = resource.LocationId;
        }

        [Member("id"), Key]
        public long Id { get; }
        
        [Member("type")]
        public MachineImageType Type { get;  }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("description")]
        [StringLength(100)]
        public string Description { get; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        [Member("locationId")]
        public long LocationId { get; }

        ResourceType IManagedResource.ResourceType => ResourceType.MachineImage;

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