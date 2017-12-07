using System;

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
            ResourceProvider provider,
            JsonObject properties = null)
        {
            Validate.Id(id);
            Validate.NotNullOrEmpty(name, nameof(name));

            Id         = id;
            Name       = name;
            ProviderId = provider.Id;
            Properties = properties ?? new JsonObject();
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("providerId")]
        public int ProviderId { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.MachineType;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        #endregion
    }
}