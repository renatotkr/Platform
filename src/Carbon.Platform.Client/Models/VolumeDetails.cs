using System;
using System.Runtime.Serialization;

using Carbon.Platform.Computing;
using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    public class VolumeDetails : IVolume
    {
        [DataMember(Name = "id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "size", Order = 2)]
        public long Size { get; set; }

        [DataMember(Name = "locationId", Order = 3)]
        public long LocationId { get; set; }

        #region IManagedResource

        int IManagedResource.ProviderId => Platform.LocationId.Create(LocationId).ProviderId;

        ResourceType IResource.ResourceType => ResourceType.Volume;

        string IManagedResource.ResourceId => throw new NotImplementedException();

        #endregion

    }
}
