using System.Runtime.Serialization;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public class VolumeDetails : IVolume
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "size", EmitDefaultValue = false)]
        public long Size { get; set; }

        [DataMember(Name = "resource")]
        public ManagedResource Resource { get; set; }

        #region IManagedResource

        ResourceType IResource.ResourceType => ResourceTypes.Volume;

        #endregion
    }
}
