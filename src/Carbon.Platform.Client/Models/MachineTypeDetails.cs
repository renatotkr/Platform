using System.Runtime.Serialization;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [DataContract]
    public class MachineTypeDetails : IMachineType
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.MachineType;

        #endregion
    }
}