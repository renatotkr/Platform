using System;
using System.Runtime.Serialization;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class DomainRegistrationDetails : IResource
    {
        [DataMember(Name = "id", EmitDefaultValue = false, Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "registarId", EmitDefaultValue = false, Order = 1)]
        public long RegistarId { get; set; }

        [DataMember(Name = "expires")]
        public DateTime Expires { get; set; }

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.DomainRegistration;

        #endregion
    }
}