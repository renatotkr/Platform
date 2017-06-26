using System.Runtime.Serialization;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class ImageDetails : IImage
    {
        [DataMember(Name = "id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "type", Order = 2)]
        public ImageType Type { get; set; }

        [DataMember(Name = "name", Order = 3)]
        public string Name { get; set; }

        [DataMember(Name = "name", Order = 4)]
        public ManagedResource Resource { get; set; }

        #region IResource

        int IManagedResource.ProviderId => Resource.ProviderId;

        int IManagedResource.LocationId => Resource.LocationId;

        string IManagedResource.ResourceId => Resource.ResourceId;

        ResourceType IResource.ResourceType => ResourceTypes.Image;

        #endregion
    }
}
