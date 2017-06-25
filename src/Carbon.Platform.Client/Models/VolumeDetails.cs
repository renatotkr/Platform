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

        [DataMember(Name = "locationId", Order = 3)]
        public int LocationId { get; set; }

        [DataMember(Name = "resourceId", Order = 5, EmitDefaultValue = false)]
        public string ResourceId { get; set; }

        #region IManagedResource

        int IManagedResource.ProviderId => Platform.LocationId.Create(LocationId).ProviderId;

        ResourceType IResource.ResourceType => ResourceTypes.Volume;

        #endregion
    }
}
