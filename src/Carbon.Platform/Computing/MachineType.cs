using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;
    using Json;

    [Dataset("MachineTypes")]
    [DataIndex(IndexFlags.Unique, new[] { "providerId", "name" })]
    public class MachineType : IMachineType, IManagedResource
    {
        [Member("id"), Key]
        public long Id { get; set; }

        [Member("name")]
        public string Name { get; set; }
        
        [Member("details", TypeName = "varchar(1000)")]
        public JsonObject Details { get; set; }

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; set; }

        string IManagedResource.ResourceId => Name;

        ResourceType IManagedResource.Type => ResourceType.MachineType;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        [TimePrecision(TimePrecision.Second)]
        public DateTime Created { get; }

        #endregion
    }
}