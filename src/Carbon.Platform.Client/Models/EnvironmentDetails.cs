using System.Runtime.Serialization;
using Carbon.Platform.Environments;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class EnvironmentDetails : IEnvironment
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Environment;

        #endregion
    }
}