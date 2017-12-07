using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("Images", Schema = "Computing")]
    [UniqueIndex("providerId", "resourceId")]
    [UniqueIndex("ownerId", "name")]
    public class ImageInfo : IImage
    {
        public ImageInfo() { }

        public ImageInfo(
            long id,
            long ownerId,
            string name,
            ImageType type,
            long size,
            ManagedResource resource,
            JsonObject properties = null)
        {
            Validate.Id(id);
            Validate.Id(ownerId, nameof(ownerId));
            Validate.NotNullOrEmpty(name, nameof(name));
            
            Id         = id;
            OwnerId    = ownerId;
            Type       = type;
            Name       = name;
            Size       = size;
            ResourceId = resource.ResourceId;
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
            Properties = properties;
        }

        [Member("id"), Key(sequenceName: "imageId")]
        public long Id { get; }
        
        [Member("type")]
        public ImageType Type { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

        [Member("name")]
        [StringLength(150)]
        public string Name { get; }

        [Member("size")]
        public long Size { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IResource

        // docker:hub/ | aws |
        [Member("providerId")]
        public int ProviderId { get; }

        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.Image;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }
        
        #endregion
    }
}