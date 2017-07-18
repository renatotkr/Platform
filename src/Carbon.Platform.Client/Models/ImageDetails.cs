using System.Runtime.Serialization;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class ImageDetails : IImage
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "type")]
        public ImageType Type { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "size", EmitDefaultValue = false)]
        public long Size { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public ManagedResource Resource { get; set; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Image;

        #endregion
    }
}
