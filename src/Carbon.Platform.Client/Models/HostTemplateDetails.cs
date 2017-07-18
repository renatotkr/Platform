using System.Runtime.Serialization;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class HostTemplateDetails : IHostTemplate
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "image")]
        public ImageDetails Image { get; set; }

        [DataMember(Name = "machineType")]
        public MachineTypeDetails MachineType { get; set; }

        [DataMember(Name = "startupScript")]
        public string StartupScript { get; set; }

        [DataMember(Name = "properties")]
        public JsonObject Properties { get; set; }

        #region IHostTemplate
        
        long IHostTemplate.ImageId => Image.Id;

        long IHostTemplate.MachineTypeId => MachineType.Id;

        #endregion

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.HostTemplate;

        #endregion
    }
}