using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Storage
{
    using Data.Annotations;
    using Json;

    [Dataset("Volumes")]
    [DataIndex(IndexFlags.Unique, "providerId", "name")]
    public class VolumeInfo : ICloudResource
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("status"), Mutable]
        public VolumeStatus Status { get; set; }

        // long TypeId

        [Member("providerId")]
        public int ProviderId { get; set; }

        [Member("name")]
        [Ascii, StringLength(100)]
        public string Name { get; set; }

        [Member("size")]
        public long Size { get; set; } // in octets

        [Member("locationId")]
        public long LocationId { get; set; }

        [Member("hostId"), Mutable]
        public long? HostId { get; set; }
        
        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }
        
        [Member("created")]
        public DateTime Created { get; set; }

        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; set; }
        
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }

        #region Helpers

        [IgnoreDataMember]
        public bool IsDeleted => Deleted != null;

        #endregion

        #region IResource

        ResourceType ICloudResource.Type => ResourceType.Volume;

        [IgnoreDataMember]
        public CloudProvider Provider => CloudProvider.Get(ProviderId);

        #endregion
    }
}

// Google Notes:
// compute#disk