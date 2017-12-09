using System.Runtime.Serialization;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [DataContract]
    public class ImageDetails : IImage
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        [DataMember(Name = "type", EmitDefaultValue = false)]
        public ImageType Type { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
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
