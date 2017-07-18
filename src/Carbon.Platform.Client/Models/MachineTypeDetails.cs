using System.Runtime.Serialization;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class MachineTypeDetails : IMachineType
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
        
        #region IResource
        
        ResourceType IResource.ResourceType => ResourceTypes.MachineType;

        #endregion

    }
}