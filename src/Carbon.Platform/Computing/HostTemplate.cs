using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.Platform.Computing
{
    [Dataset("HostTemplates")]
    public class HostTemplate : IHostTemplate
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("machineTypeId")]
        public long MachineTypeId { get; set; }

        [Member("machineImageId")]
        public long MachineImageId { get; set; }

        // Script at startup
        [Member("script")]
        public string Script { get; set; }

        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; set; }

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; set; }

        [IgnoreDataMember]
        [Member("resourceId")]
        [Ascii, StringLength(100)]
        public string ResourceId { get; set; }

        long IManagedResource.LocationId => 0;

        ResourceType IManagedResource.ResourceType => ResourceType.HostTemplate;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted"), Timestamp]
        public DateTime? Deleted { get; }

        #endregion
    }
}
