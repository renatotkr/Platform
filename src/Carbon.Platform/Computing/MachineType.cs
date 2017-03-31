using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;
    using Json;

    [Dataset("MachineTypes")]
    [DataIndex(IndexFlags.Unique, new[] { "providerId", "name" })]
    public class MachineType : IMachineType, ICloudResource
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

        string ICloudResource.ResourceId => Name;

        ResourceType ICloudResource.Type => ResourceType.MachineType;

        #endregion

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }
    }
}