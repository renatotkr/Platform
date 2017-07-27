using System.Runtime.Serialization;
using Carbon.Platform.Computing;
using Carbon.Platform.Environments;
using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    public class EnvironmentDetails : IEnvironment
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(Name = "ownerId", EmitDefaultValue = false)]
        public long OwnerId { get; set; }

        [DataMember(Name = "clusters", EmitDefaultValue = false)]
        public ClusterDetails[] Clusters { get; set; }

        [DataMember(Name = "programs", EmitDefaultValue = false)]
        public ProgramDetails[] Programs { get; set; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Environment;

        #endregion
    }
}