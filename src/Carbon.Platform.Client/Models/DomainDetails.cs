using System.Runtime.Serialization;
using Carbon.Platform.Hosting;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [DataContract]
    public class DomainDetails : IDomain, IResource
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "path", EmitDefaultValue = false)]
        public string Path { get; set; }

        [DataMember(Name = "ownerId", EmitDefaultValue = false)]
        public long OwnerId { get; set; }
        
        [DataMember(Name = "registration", EmitDefaultValue = false)]
        public DomainRegistrationDetails Registration { get; set; }

        [DataMember(Name = "records", EmitDefaultValue = false)]
        public DomainRecordDetails[] Records { get; set; }
        
        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Domain;

        #endregion
    }
}