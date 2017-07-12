using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("MachineTypes", Schema = "Computing")]
    [UniqueIndex("providerId", "name")]
    public class MachineType : IMachineType
    {
        public MachineType() { }

        public MachineType(
            long id, 
            string name,
            JsonObject properties, 
            ResourceProvider provider)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            #endregion

            Id         = id;
            Name       = name ?? throw new ArgumentNullException(nameof(name));
            Properties = properties;
            ProviderId = provider.Id;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }
        
        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; }

        ResourceType IResource.ResourceType => ResourceTypes.MachineType;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        #endregion
    }
}