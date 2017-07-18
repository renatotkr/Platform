using System.Runtime.Serialization;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class ClusterDetails : ICluster
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
        
        [DataMember(Name = "location")]
        public LocationDetails Location { get; set; }

        [DataMember(Name = "environment")]
        public EnvironmentDetails Environment { get; set; }

        [DataMember(Name = "hostTemplate")]
        public HostTemplateDetails HostTemplate { get; set; }

        // Hosts ...
        
        #region ICluster

        int ICluster.LocationId => Location.Id;

        long ICluster.EnvironmentId => Environment.Id;

        #endregion

        #region IResource
        
        ResourceType IResource.ResourceType => ResourceTypes.Cluster;

        #endregion
    }
}