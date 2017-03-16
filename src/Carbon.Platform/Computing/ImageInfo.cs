using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;
    using Json;

    [Dataset("Images")]
    [DataIndex(IndexFlags.Unique, "providerId", "name")]
    public class ImageInfo : ICloudResource
    {
        [Member("id"), Identity]
        public long Id { get; set; }

        [Member("providerId")]
        public int ProviderId { get; set; }

        [Member("name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Member("type")]
        public ImageType Type { get; set; }

        [Member("details")]
        [StringLength(1000)] // Size?
        public JsonObject Details { get; set; }

        [Member("created"), Timestamp]
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

        ResourceType ICloudResource.Type => ResourceType.Image;

        [IgnoreDataMember]
        public CloudProvider Provider => CloudProvider.Get(ProviderId);

        #endregion
    }

    // Windows/x86-64#patch?

    // AWS: ami

    // Details
    // - architechure
    // - platform               /x64/Windows/2016#patchLevel
    // - hypervisor
    // - repository



    // e.g. sha256:77af4d6b9913e693e8d0b4b294fa62ade6054e6b2f1ffb617ac955dd63fb0182

    public enum ImageType : byte
    {
        Kernel    = 1,
        Machine   = 2, // ami (amazon machine image)
        Container = 3,
    
        // RamDisk ?
    }

    public enum ImageStatus : byte
    {
        Pending   = 0,
        Available = 1,
        Deleted   = 3
    }
}