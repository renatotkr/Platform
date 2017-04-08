using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Computing
{
    using Data.Annotations;
    using Json;

    [Dataset("MachineTypes")]
    [DataIndex(IndexFlags.Unique, new[] { "providerId", "name" })]
    public class MachineType : IMachineType
    {
        public MachineType() { }

        public MachineType(long id, string name, ResourceProvider provider)
        {
            Id         = id;
            Name       = name ?? throw new ArgumentNullException(nameof(name));
            ProviderId = provider.Id;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }
        
        [Member("details", TypeName = "varchar(1000)")]
        public JsonObject Details { get; set; }

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; }

        string IManagedResource.ResourceId => Name;

        ResourceType IManagedResource.ResourceType => ResourceType.MachineType;

        long IManagedResource.LocationId => 0;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        [TimePrecision(TimePrecision.Second)]
        public DateTime Created { get; }

        #endregion
    }
}