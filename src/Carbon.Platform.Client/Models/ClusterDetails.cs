using System;
using System.Runtime.Serialization;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class ClusterDetails : ICluster
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }
        
        [DataMember(Name = "location", EmitDefaultValue = false)]
        public LocationDetails Location { get; set; }

        [DataMember(Name = "environment", EmitDefaultValue = false)]
        public EnvironmentDetails Environment { get; set; }

        [DataMember(Name = "hostTemplate", EmitDefaultValue = false)]
        public HostTemplateDetails HostTemplate { get; set; }

        [DataMember(Name = "hosts", EmitDefaultValue = false)]
        public HostDetails[] Hosts { get; set; }

        #region ICluster

        int ICluster.LocationId => Location.Id;

        long ICluster.EnvironmentId => Environment.Id;

        #endregion

        #region IResource
        
        ResourceType IResource.ResourceType => ResourceTypes.Cluster;

        #endregion
    }
}