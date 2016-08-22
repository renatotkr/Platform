namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "Zones")]
    public class ZoneInfo
    {
        [Member(1), Identity]
        public long Id { get; }

        [Member(2), Unique] // aws/us-east-1/A
        public string Path { get; set; }

        [Member(3)]
        public ZoneLevel Level { get; }
    }

    public enum ZoneLevel
    {
        Cloud      = 0, // aws
        Region     = 1, // us-west1
        Datacenter = 2, // A
        Rack       = 3  // 352
    }
}

/*
Google regions

us-west1	        10.138.0.0/20	10.138.0.1
us-central1	        10.128.0.0/20	10.128.0.1
us-east1	        10.142.0.0/20	10.142.0.1
europe-west1	    10.132.0.0/20	10.132.0.1
asia-east1	        10.140.0.0/20	10.140.0.1

ref:  https://cloud.google.com/compute/docs/regions-zones/regions-zones

----------------------------------------------------------------------------------

AWS REGIONS

US East(N.Virginia)         us-east-1	        US Standard US East(N.Virginia)
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
