using System.Runtime.Serialization;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Dataset("Regions")]
    [DataIndex(IndexFlags.Unique, new[] { "provider", "name" })]
    public class RegionInfo : ICloudResource
    {
        public RegionInfo() { }

        public RegionInfo(long id, CloudPlatformProvider provider, string name)
        {
            Id           = id;
            ProviderName = provider.Code;
            Name         = name;
        }

        [Member("id"), Identity]
        public long Id { get; set; }

        // e.g. amzn
        [Member("provider", TypeName = "varchar(4)")]
        [MaxLength(4)]
        public string ProviderName { get; set; }

        // e.g. us-east-1
        [Member("name")]
        public string Name { get; set; }

        [IgnoreDataMember]
        public CloudPlatformProvider Provider => CloudPlatformProvider.Parse(ProviderName);

        #region IResource

        ResourceType ICloudResource.Type => ResourceType.Region;

        #endregion
    }
}

/*
Google Regions  | https://cloud.google.com/compute/docs/regions-zones/regions-zones
----------------------------------------------------------------------------------

us-west1
us-central1
us-east1
europe-west1
asia-east1


AWS Regions
----------------------------------------------------------------------------------

US East(N.Virginia)         us-east-1	        US Standard US East (N.Virginia)
US West(Oregon)             us-west-2	        Oregon US West(Oregon)
US West(N.California)       us-west-1	        Northern California US West(N.California)
EU(Ireland)                 eu-west-1	        Ireland EU(Ireland)
EU(Frankfurt)               eu-central-1	    Frankfurt EU(Frankfurt)
Asia Pacific(Singapore)     ap-southeast-1	    Singapore Asia Pacific(Singapore)
Asia Pacific(Tokyo)         ap-northeast-1	    Tokyo Asia Pacific(Tokyo)
Asia Pacific(Seoul)         ap-northeast-2	    Seoul Asia Pacific(Seoul)
Asia Pacific(Sydney)        ap-southeast-2	    Sydney Asia Pacific(Sydney)
South America(São Paulo)    sa-east-1	        Sao Paulo   South America(São Paulo)
AWS GovCloud(US)            us-gov-west-1	    GovCloud AWS GovCloud(US)
China(Beijing)              cn-north-1	        China(Beijing) China(Beijing)

Azure has no zone equivlant...

... notes:
336 cities with over 1M people
1,127 cities with at least 500,000 inhabits | 2.317 billion people in these cities
4,116 cities with at least 100,000 people 
*/
