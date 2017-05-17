using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("MachineTypes")]
    [DataIndex(IndexFlags.Unique, new[] { "providerId", "name" })]
    public class MachineType : IMachineType
    {
        public MachineType() { }

        public MachineType(
            long id, 
            string name,
            JsonObject details, 
            ResourceProvider provider)
        {
            Id         = id;
            Name       = name ?? throw new ArgumentNullException(nameof(name));
            Details    = details;
            ProviderId = provider.Id;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }
        
        [Member("details")]
        [StringLength(1000)]
        public JsonObject Details { get; }

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.MachineType;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        [TimePrecision(TimePrecision.Second)]
        public DateTime Created { get; }

        #endregion
    }
}