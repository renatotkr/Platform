using System;
using System.Runtime.Serialization;

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
            ImageType type,
            string name,
            long ownerId,
            long size,
            ManagedResource resource,
            JsonObject properties = null)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            #endregion

            Id         = id;
            Type       = type;
            Name       = name;
            OwnerId    = ownerId;
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
        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; }

        [Member("locationId")]
        public int LocationId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.Image;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }
        
        #endregion
    }
}