using System;

using Carbon.Extensions;

namespace Carbon.Platform
{
    public class Locations 
    {
        public static Location Get(ResourceProvider provider, string name)
        {
            #region Preconditions

            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            // us-east-1a

            var parts = name.Split(Seperators.Dash);

            if (parts.Length != 3) throw new Exception("Invalid name");

            if (parts[2].Length == 2)
            {
                var zone = parts[2][1];
                
                var location = Get(provider, name.Substring(0, name.Length - 1));

                return location.WithZone(zone);
            }

            foreach (var region in AwsRegions)
            {
                if (region.Name == name) return region;
            }

            throw new Exception($"aws:location/{name} not found");
        }


        public static Location Get(int id)
        {
            var lid = LocationId.Create(id);

            if (lid.ProviderId == ResourceProvider.Aws.Id)
            {
                switch (lid.RegionNumber)
                {
                    case 1  : return Aws_US_East_1;
                    case 2  : return Aws_EU_West_1;
                    case 3  : return Aws_US_West_1;
                    case 4  : return Aws_AP_SouthEast_1;
                    case 5  : return Aws_AP_NorthEast_1;
                    case 6  : return Aws_US_GovWest_1;
                    case 7  : return Aws_US_West_2;
                    case 8  : return Aws_SA_East_1;
                    case 9  : return Aws_AP_SouthEast_2;
                    case 10 : return Aws_CN_North_1;
                    case 11 : return Aws_EU_Central_1;
                    case 12 : return Aws_AP_Northeast_2;
                    case 13 : return Aws_AP_South_1;
                    case 14 : return Aws_US_East_2;
                    case 15 : return Aws_CA_Central_1;
                    case 16 : return Aws_EU_West_2;
                }
            }

            if (lid.ProviderId == ResourceProvider.Azure.Id) // Azure
            {
                throw new Exception("Azure is not supported");
            }

            if (lid.ProviderId == ResourceProvider.Gcp.Id) // GCP
            {
                if (lid.RegionNumber == 0) // Mutli regional
                {
                    switch (lid.ZoneNumber)
                    {
                        case 1  : return Gcp_US;
                        case 2  : return Gcp_EU;
                        case 3  : return Gcp_Asia;
                        default : throw new Exception("No multi-region found:" + lid.ZoneNumber);
                    }
                }

                switch (lid.RegionNumber)
                {
                    case 1 : return Gcp_USCentral1;
                    case 2 : return Gcp_EuropeWest1;
                    case 3 : return Gcp_AsiaEast1;     
                    case 4 : return Gcp_USEast1;       
                    case 5 : return Gcp_USWest1;
                    case 6 : return Gcp_AsiaNorthEast1;
                }
            }

            throw new Exception($"Unexpected location id: {lid.ProviderId}|{lid.RegionNumber}|{lid.ZoneNumber}");
        }

        private static Location Create(ResourceProvider provider, ushort regionNumber, byte zoneNumber, string name)
        {
            return new Location(LocationId.Create(provider, regionNumber, zoneNumber), name);
        }

        private static Location Create(ResourceProvider provider, ushort regionNumber, string name)
        {
            return new Location(LocationId.Create(provider, regionNumber, 0), name);
        }

        public static readonly Location Global = new Location(LocationId.Zero, "global");

        // AWS | https://cloud.google.com/compute/docs/regions-zones/regions-zones
        //------------------------------------------------------------------------------------------------------------------------------------------
        private static readonly ResourceProvider aws = ResourceProvider.Aws;

        public static readonly Location Aws_US_East_1        = Create(aws,  1, "us-east-1");       // | US    | N. Virginia   | 2006-08-25
        public static readonly Location Aws_EU_West_1        = Create(aws,  2, "eu-west-1");       // | EU    | Ireland       | 2008-12-10
        public static readonly Location Aws_US_West_1        = Create(aws,  3, "us-west-1");       // | US    | N. California | 2009-12-03
        public static readonly Location Aws_AP_SouthEast_1   = Create(aws,  4, "ap-southeast-1");  // | AP    | Singapore     | 2010-04-29
        public static readonly Location Aws_AP_NorthEast_1   = Create(aws,  5, "ap-northeast-1");  // | AP    | Tokyo         | 2011-03-02
        public static readonly Location Aws_US_GovWest_1     = Create(aws,  6, "us-gov-west-1");   // | US    | AWS GovCloud  | 2011-08-16
        public static readonly Location Aws_US_West_2        = Create(aws,  7, "us-west-2");       // | US    | Oregon        | 2011-11-09
        public static readonly Location Aws_SA_East_1        = Create(aws,  8, "sa-east-1");       // | SA    | São Paulo     | 2011-12-14
        public static readonly Location Aws_AP_SouthEast_2   = Create(aws,  9, "ap-southeast-2");  // | AP    | Sydney        | 2012-11-12
        public static readonly Location Aws_CN_North_1       = Create(aws, 10, "cn-north-1");      // | China | Beijing       | 2013-12-18
        public static readonly Location Aws_EU_Central_1     = Create(aws, 11, "eu-central-1");    // | EU    | Frankfurt     | 2014-10-23
        public static readonly Location Aws_AP_Northeast_2   = Create(aws, 12, "ap-northeast-2");  // | AP    | Seoul	        | 2016-01-06
        public static readonly Location Aws_AP_South_1       = Create(aws, 13, "ap-south-1");      // | AP    | Mumbai        | 2016-06-27
        public static readonly Location Aws_US_East_2        = Create(aws, 14, "us-east-2");       // | US    | Ohio          | 2016-10-17
        public static readonly Location Aws_CA_Central_1     = Create(aws, 15, "ca-central-1");    // | CA    | Central       | 2016-12-08
        public static readonly Location Aws_EU_West_2        = Create(aws, 16, "eu-west-2");       // | EU    | London        | 2016-12-13

        /*
        Paris
        Ningxia
        Stockholm
        */

        // Azure (https://azure.microsoft.com/en-us/regions/) -- announced 2008, launched 2010-01
        // ------------------------------------------------------------------------------------------------------------------------------------------

        // Not settled on these ids yet... but close.

        private static readonly ResourceProvider azure = ResourceProvider.Azure;

        public static readonly Location Azure_US_NorthCentral = Create(azure,  1, "North Central US");   // | US/IL       | ~ 2010
        public static readonly Location Azure_US_SouthCentral = Create(azure,  2, "South Central US");   // | US/TX       | ~ 2010
        public static readonly Location Azure_EU_North        = Create(azure,  3, "North Europe");       // | Ireland     | ?
        public static readonly Location Azure_EU_West         = Create(azure,  4, "West Europe");        // | Netherlands | ? 
        public static readonly Location Azure_AP_East         = Create(azure,  5, "East Asia");          // | Hong Kong   | ? < 2010-05-01
        public static readonly Location Azure_AP_SouthEast    = Create(azure,  6, "Southeast Asia");     // | Singapore   | ? < 2010-05-01 
        public static readonly Location Azure_US_East         = Create(azure,  7, "East US");            // | US/Virginia | ? < 2011-06-06
        public static readonly Location Azure_US_West         = Create(azure,  8, "West US");            // | US/CA       | ? < 2011-06-06
        public static readonly Location Azure_JP_East         = Create(azure,  9, "Japan East");         // | Saitama     | 2014-02-25 GA
        public static readonly Location Azure_JP_West         = Create(azure, 10, "Japan West");         // | Osaka       | 2014-02-25 GA    
        
        public static readonly Location Azure_CN_East         = Create(azure, 11, "China East");          // | Shanghai    | 2014-03-27
        public static readonly Location Azure_CN_North        = Create(azure, 12, "China North");         // | Beijing     | 2014-03-27      
        public static readonly Location Azure_BR_South        = Create(azure, 13, "Brazil South");        // | Sao Paulo   | 2014-04-17 PV 
        public static readonly Location Azure_US_East2        = Create(azure, 14, "East US 2");           // | US/Virginia | 2014-07-09 ?
        public static readonly Location Azure_US_WestCentral  = Create(azure, 15, "West Central US");     // | US/?
        public static readonly Location Azure_US_Central      = Create(azure, 16, "Central US");          // | US/IA       | 2014-07-09 ?
        public static readonly Location Azure_AU_East	      = Create(azure, 17, "Australia East");      // | AU/NSW      | 2014-10-27 GA
        public static readonly Location Azure_AU_SouthEast    = Create(azure, 18, "Australia Southeast"); // | Victoria    | 2014-10-27 GA
        public static readonly Location Azure_US_GOV_Virgina  = Create(azure, 19, "US Gov Virginia");     // | US/Virginia | 2014-12-09 GA 
        public static readonly Location Azure_US_GOV_Iowa     = Create(azure, 20, "US Gov Iowa");         // | US/Iowa     | 2014-12-09 GA
        public static readonly Location Azure_IN_Central	  = Create(azure, 21, "Central India");       // | Pune        | 2015-09-29 ?
        public static readonly Location Azure_IN_West	      = Create(azure, 22, "West India");          // | Mumbai      | 2015-09-29 ?
        public static readonly Location Azure_IN_South	      = Create(azure, 23, "South India");         // | Chennai     | 2015-09-29 ?
        public static readonly Location Azure_DE_Central      = Create(azure, 24, "Germany Central");     // | Frankfurt   | 2016-03-15
        public static readonly Location Azure_DE_NorthEast    = Create(azure, 25, "Germany Northeast");   // | Magdeburg   | 2016-03-15  
        public static readonly Location Azure_CA_East         = Create(azure, 26, "Canada East");         // | Quebec City | 2016-05-10 GA
        public static readonly Location Azure_CA_Central      = Create(azure, 27, "Canada Central");      // | Toronto     | 2016-05-10 GA
        public static readonly Location Azure_US_West2        = Create(azure, 28, "West US 2");           // | US          | 2016-07-13 GA
        public static readonly Location Azure_UK_West         = Create(azure, 29, "UK West");             // | Cardiff     | 2016-09-07 GA
        public static readonly Location Azure_UK_South        = Create(azure, 30, "UK South");            // | London      | 2016-09-07 GA
        public static readonly Location Azure_US_DoD_East     = Create(azure, 31, "US DoD East");         // | US/?        | 2016-10-18 ?
        public static readonly Location Azure_US_DoD_Central  = Create(azure, 32, "US DoD Central");      // | US?         | 2016-10-18 ?
        public static readonly Location Azure_KR_Central      = Create(azure, 33, "Korea Central");       // | Seoul       | 2017-02-21 GA
        public static readonly Location Azure_KR_South        = Create(azure, 34, "Korea South");         // | Busan       | 2017-02-21 GA

        // As of Feb 28th, 2016

        // Brazil South : 2014-06-05 GA   
        // US Central   : 2014-07-14 GA
        // US East 2    : 2014-07-14 GA

        // SOON
        public static readonly Location Azure_FR_Central     = Create(azure, 35, "France Central");        // | 2016-10-03
        public static readonly Location Azure_FR_South       = Create(azure, 36, "France South");          // | 2016-10-03
        public static readonly Location Azure_US_GOV_AZ      = Create(azure, 37, "US Gov Arizona");
        public static readonly Location Azure_US_GOV_TT      = Create(azure, 38, "US Gov Texas");


        // GCP | https://cloud.google.com/compute/docs/regions-zones/regions-zones
        // ------------------------------------------------------------------------------------------------------------------------------------------

        private static readonly ResourceProvider gcp = ResourceProvider.Gcp;

        // Multi-Regions
        public static readonly Location Gcp_US             = new Location(LocationId.Create(gcp, 0, 1), "us"); 
        public static readonly Location Gcp_EU             = new Location(LocationId.Create(gcp, 0, 2), "eu"); 
        public static readonly Location Gcp_Asia           = new Location(LocationId.Create(gcp, 0, 3), "asia");

        // Regions                                                                                                  
        public static readonly Location Gcp_USCentral1     = Create(gcp, 1, "us-central1");     // | US    | Iowa          | ?
        public static readonly Location Gcp_EuropeWest1    = Create(gcp, 2, "europe-west1");    // | EU    | Belgium       | ?
        public static readonly Location Gcp_AsiaEast1      = Create(gcp, 3, "asia-east1");      // | AP    | Taiwan        | 2014-04-07
        public static readonly Location Gcp_USEast1        = Create(gcp, 4, "us-east1");        // | US    | South Caroli. | 2015-10-01
        public static readonly Location Gcp_USWest1        = Create(gcp, 5, "us-west1");        // | US    | Oregon        | 2016-06-20 
        public static readonly Location Gcp_AsiaNorthEast1 = Create(gcp, 6, "asia-northeast1"); // | AP    | Tokyo         | 2016-11-08
        public static readonly Location Gcp_AsiaSouthEast1 = Create(gcp, 7, "asia-southeast1"); // | AP    | Singapore     | 2017-04-??
        public static readonly Location Gcp_USEast4        = Create(gcp, 8, "us-east4");        // | US    | N. Virgina    | 2017-05-10


        public static readonly Location[] AwsRegions = new[] {
            Aws_US_East_1,
            Aws_EU_West_1,
            Aws_US_West_1,
            Aws_AP_SouthEast_1,
            Aws_AP_NorthEast_1,
            Aws_US_GovWest_1,
            Aws_US_West_2,
            Aws_SA_East_1,
            Aws_AP_SouthEast_2,
            Aws_CN_North_1,
            Aws_EU_Central_1,
            Aws_AP_Northeast_2,
            Aws_AP_South_1,
            Aws_US_East_2,
            Aws_CA_Central_1,
            Aws_EU_West_2
        };

        public static readonly Location[] GcpRegions = new[] {
            Gcp_USCentral1,
            Gcp_EuropeWest1,
            Gcp_AsiaEast1,
            Gcp_USEast1,
            Gcp_USWest1,
            Gcp_AsiaNorthEast1,
            Gcp_AsiaSouthEast1,
            Gcp_USEast4
        };
    }  
}