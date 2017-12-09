using System.Runtime.Serialization;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [DataContract]
    public class HostTemplateDetails : IHostTemplate
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(Name = "image", EmitDefaultValue = false)]
        public ImageDetails Image { get; set; }

        [DataMember(Name = "machineType", EmitDefaultValue = false)]
        public MachineTypeDetails MachineType { get; set; }

        [DataMember(Name = "startupScript", EmitDefaultValue = false)]
        public string StartupScript { get; set; }

        [DataMember(Name = "properties", EmitDefaultValue = false)]
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