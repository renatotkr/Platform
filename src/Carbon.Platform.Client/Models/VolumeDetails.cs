using System.Runtime.Serialization;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public class VolumeDetails : IVolume
    {
        [DataMember(Name = "id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "size", Order = 2)]
        public long Size { get; set; }

        [DataMember(Name = "resource", Order = 4)]
        public ManagedResource Resource { get; set; }

        #region IManagedResource

        string IManagedResource.ResourceId => Resource.ResourceId;

        int IManagedResource.ProviderId => Resource.ProviderId;

        int IManagedResource.LocationId => Resource.LocationId;

        ResourceType IResource.ResourceType => ResourceTypes.Volume;

        #endregion
    }
}
