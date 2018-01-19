using System.Runtime.Serialization;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public sealed class AwsMachineType : IMachineType
    {
        internal AwsMachineType(long id, string name)
        {
            Id   = id;
            Name = name;
        }

        [DataMember(Name = "id")]
        public long Id { get; } 

        [DataMember(Name = "name")]
        public string Name { get; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.MachineType;

        #endregion
    }
}