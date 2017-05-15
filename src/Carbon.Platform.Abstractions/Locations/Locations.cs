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

            foreach (var region in AmazonRegions)
            {
                if (region.Name == name) return region;
            }

            throw new Exception("no amazon region found for:" + name);
        }


        public static Location Get(int id)
        {
            var lid = LocationId.Create(id);

            if (lid.ProviderId == ResourceProvider.Aws.Id)
            {
                switch (lid.RegionNumber)
                {
                    case 1  : return Amazon_US_East1;
                    case 2  : return Amazon_EU_West1;
                    case 3  : return Amazon_US_West1;
                    case 4  : return Amazon_AP_SouthEast1;
                    case 5  : return Amazon_AP_NorthEast1;
                    case 6  : return Amazon_US_GovWest1;
                    case 7  : return Amazon_US_West2;
                    case 8  : return Amazon_SA_East1;
                    case 9  : return Amazon_AP_SouthEast2;
                    case 10 : return Amazon_CN_North1;
                    case 11 : return Amazon_EU_Central1;
                    case 12 : return Amazon_AP_Northeast2;
                    case 13 : return Amazon_AP_South1;
                    case 14 : return Amazon_US_East2;
                    case 15 : return Amazon_CA_Central1;
                    case 16 : return Amazon_EU_West2;
                }
            }

            if (lid.ProviderId == ResourceProvider.Microsoft.Id)
            {
                throw new Exception("Microsoft not yet supported");
            }

            if (lid.ProviderId == ResourceProvider.Gcp.Id) // Google
            {
                if (lid.RegionNumber == 0) // Mutli regional
                {
                    switch (lid.ZoneNumber)
                    {
                        case 1  : return Google_US;
                        case 2  : return Google_EU;
                        case 3  : return Google_Asia;
                        default : throw new Exception("No multi-region:" + lid.ZoneNumber);
                    }
                }

                switch (lid.RegionNumber)
                {
                    case 1 : return Google_USCentral1;
                    case 2 : return Google_EuropeWest1;
                    case 3 : return Google_AsiaEast1;     
                    case 4 : return Google_USEast1;       
                    case 5 : return Google_USWest1;
                    case 6 : return Google_AsiaNorthEast1;
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

        // Amazon | https://cloud.google.com/compute/docs/regions-zones/regions-zones
        //------------------------------------------------------------------------------------------------------------------------------------------
        private static readonly ResourceProvider aws = ResourceProvider.Aws;

        public static readonly Location Amazon_US_East1        = Create(aws,  1, "us-east-1");       // | US    | N. Virginia   | 2006-08-25
        public static readonly Location Amazon_EU_West1        = Create(aws,  2, "eu-west-1");       // | EU    | Ireland       | 2008-12-10
        public static readonly Location Amazon_US_West1        = Create(aws,  3, "us-west-1");       // | US    | N. California | 2009-12-03
        public static readonly Location Amazon_AP_SouthEast1   = Create(aws,  4, "ap-southeast-1");  // | AP    | Singapore     | 2010-04-29
        public static readonly Location Amazon_AP_NorthEast1   = Create(aws,  5, "ap-northeast-1");  // | AP    | Tokyo         | 2011-03-02
        public static readonly Location Amazon_US_GovWest1     = Create(aws,  6, "us-gov-west-1");   // | US    | AWS GovCloud  | 2011-08-16
        public static readonly Location Amazon_US_West2        = Create(aws,  7, "us-west-2");       // | US    | Oregon        | 2011-11-09
        public static readonly Location Amazon_SA_East1        = Create(aws,  8, "sa-east-1");       // | SA    | São Paulo     | 2011-12-14
        public static readonly Location Amazon_AP_SouthEast2   = Create(aws,  9, "ap-southeast-2");  // | AP    | Sydney        | 2012-11-12
        public static readonly Location Amazon_CN_North1       = Create(aws, 10, "cn-north-1");      // | China | Beijing       | 2013-12-18
        public static readonly Location Amazon_EU_Central1     = Create(aws, 11, "eu-central-1");    // | EU    | Frankfurt     | 2014-10-23
        public static readonly Location Amazon_AP_Northeast2   = Create(aws, 12, "ap-northeast-2");  // | AP    | Seoul	        | 2016-01-06
        public static readonly Location Amazon_AP_South1       = Create(aws, 13, "ap-south-1");      // | AP    | Mumbai        | 2016-06-27
        public static readonly Location Amazon_US_East2        = Create(aws, 14, "us-east-2");       // | US    | Ohio          | 2016-10-17
        public static readonly Location Amazon_CA_Central1     = Create(aws, 15, "ca-central-1");    // | CA    | Central       | 2016-12-08
        public static readonly Location Amazon_EU_West2        = Create(aws, 16, "eu-west-2");       // | EU    | London        | 2016-12-13

        // Azure (https://azure.microsoft.com/en-us/regions/) -- announced 2008, launched 2010-01
        // ------------------------------------------------------------------------------------------------------------------------------------------
        
        // Not settled on these ids yet... but close.

        private static readonly ResourceProvider azure = ResourceProvider.Microsoft;

        public static readonly Location Microsoft_US_NorthCentral = Create(azure,  1, "North Central US");   // | US/IL       | ~ 2010
        public static readonly Location Microsoft_US_SouthCentral = Create(azure,  2, "South Central US");   // | US/TX       | ~ 2010
        public static readonly Location Microsoft_EU_North        = Create(azure,  3, "North Europe");       // | Ireland     | ?
        public static readonly Location Microsoft_EU_West         = Create(azure,  4, "West Europe");        // | Netherlands | ? 
        public static readonly Location Microsoft_AP_East         = Create(azure,  5, "East Asia");          // | Hong Kong   | ? < 2010-05-01
        public static readonly Location Microsoft_AP_SouthEast    = Create(azure,  6, "Southeast Asia");     // | Singapore   | ? < 2010-05-01 
        public static readonly Location Microsoft_US_East         = Create(azure,  7, "East US");            // | US/Virginia | ? < 2011-06-06
        public static readonly Location Microsoft_US_West         = Create(azure,  8, "West US");            // | US/CA       | ? < 2011-06-06
        public static readonly Location Microsoft_JP_East         = Create(azure,  9, "Japan East");         // | Saitama     | 2014-02-25 GA
        public static readonly Location Microsoft_JP_West         = Create(azure, 10, "Japan West");         // | Osaka       | 2014-02-25 GA    
        
        public static readonly Location Microsoft_CN_East         = Create(azure, 11, "China East");          // | Shanghai    | 2014-03-27
        public static readonly Location Microsoft_CN_North        = Create(azure, 12, "China North");         // | Beijing     | 2014-03-27      
        public static readonly Location Microsoft_BR_South        = Create(azure, 13, "Brazil South");        // | Sao Paulo   | 2014-04-17 PV 
        public static readonly Location Microsoft_US_East2        = Create(azure, 14, "East US 2");           // | US/Virginia | 2014-07-09 ?
        public static readonly Location Microsoft_US_WestCentral  = Create(azure, 15, "West Central US");     // | US/?
        public static readonly Location Microsoft_US_Central      = Create(azure, 16, "Central US");          // | US/IA       | 2014-07-09 ?
        public static readonly Location Microsoft_AU_East	      = Create(azure, 17, "Australia East");      // | AU/NSW      | 2014-10-27 GA
        public static readonly Location Microsoft_AU_SouthEast    = Create(azure, 18, "Australia Southeast"); // | Victoria    | 2014-10-27 GA
        public static readonly Location Microsoft_US_GOV_Virgina  = Create(azure, 19, "US Gov Virginia");     // | US/Virginia | 2014-12-09 GA 
        public static readonly Location Microsoft_US_GOV_Iowa     = Create(azure, 20, "US Gov Iowa");         // | US/Iowa     | 2014-12-09 GA
        public static readonly Location Microsoft_IN_Central	  = Create(azure, 21, "Central India");       // | Pune        | 2015-09-29 ?
        public static readonly Location Microsoft_IN_West	      = Create(azure, 22, "West India");          // | Mumbai      | 2015-09-29 ?
        public static readonly Location Microsoft_IN_South	      = Create(azure, 23, "South India");         // | Chennai     | 2015-09-29 ?
        public static readonly Location Microsoft_DE_Central      = Create(azure, 24, "Germany Central");     // | Frankfurt   | 2016-03-15
        public static readonly Location Microsoft_DE_NorthEast    = Create(azure, 25, "Germany Northeast");   // | Magdeburg   | 2016-03-15  
        public static readonly Location Microsoft_CA_East         = Create(azure, 26, "Canada East");         // | Quebec City | 2016-05-10 GA
        public static readonly Location Microsoft_CA_Central      = Create(azure, 27, "Canada Central");      // | Toronto     | 2016-05-10 GA
        public static readonly Location Microsoft_US_West2        = Create(azure, 28, "West US 2");           // | US          | 2016-07-13 GA
        public static readonly Location Microsoft_UK_West         = Create(azure, 29, "UK West");             // | Cardiff     | 2016-09-07 GA
        public static readonly Location Microsoft_UK_South        = Create(azure, 30, "UK South");            // | London      | 2016-09-07 GA
        public static readonly Location Microsoft_US_DoD_East     = Create(azure, 31, "US DoD East");         // | US/?        | 2016-10-18 ?
        public static readonly Location Microsoft_US_DoD_Central  = Create(azure, 32, "US DoD Central");      // | US?         | 2016-10-18 ?
        public static readonly Location Microsoft_KR_Central      = Create(azure, 33, "Korea Central");       // | Seoul       | 2017-02-21 GA
        public static readonly Location Microsoft_KR_South        = Create(azure, 34, "Korea South");         // | Busan       | 2017-02-21 GA

        // As of Feb 28th, 2016

        // Brazil South : 2014-06-05 GA   
        // US Central   : 2014-07-14 GA
        // US East 2    : 2014-07-14 GA

        // SOON
        public static readonly Location Microsoft_FR_Central     = Create(azure, 35, "France Central");        // | 2016-10-03
        public static readonly Location Microsoft_FR_South       = Create(azure, 36, "France South");          // | 2016-10-03
        public static readonly Location Microsoft_US_GOV_AZ      = Create(azure, 37, "US Gov Arizona");
        public static readonly Location Microsoft_US_GOV_TT      = Create(azure, 38, "US Gov Texas");


        // Google | https://cloud.google.com/compute/docs/regions-zones/regions-zones
        // ------------------------------------------------------------------------------------------------------------------------------------------

        private static readonly ResourceProvider google = ResourceProvider.Gcp;

        // Multi-Regions
        public static readonly Location Google_US             = new Location(LocationId.Create(google, 0, 1), "us"); 
        public static readonly Location Google_EU             = new Location(LocationId.Create(google, 0, 2), "eu"); 
        public static readonly Location Google_Asia           = new Location(LocationId.Create(google, 0, 3), "asia");

        // Regions                                                                                                  
        public static readonly Location Google_USCentral1     = Create(google, 1, "us-central1");     // | US    | Iowa          | ?
        public static readonly Location Google_EuropeWest1    = Create(google, 2, "europe-west1");    // | EU    | Belgium       | ?
        public static readonly Location Google_AsiaEast1      = Create(google, 3, "asia-east1");      // | AP    | Taiwan        | 2014-04-07
        public static readonly Location Google_USEast1        = Create(google, 4, "us-east1");        // | US    | South Caroli. | 2015-10-01
        public static readonly Location Google_USWest1        = Create(google, 5, "us-west1");        // | US    | Oregon        | 2016-06-20 
        public static readonly Location Google_AsiaNorthEast1 = Create(google, 6, "asia-northeast1"); // | AP    | Tokyo         | 2016-11-08

    

        public static readonly Location[] GoogleRegions = new[] {
            Google_USCentral1,
            Google_EuropeWest1,
            Google_AsiaEast1,
            Google_USEast1,
            Google_USWest1,
            Google_AsiaNorthEast1
        };

        public static readonly Location[] AmazonRegions = new[] {
            Amazon_US_East1,
            Amazon_EU_West1,
            Amazon_US_West1,
            Amazon_AP_SouthEast1,
            Amazon_AP_NorthEast1,
            Amazon_US_GovWest1,
            Amazon_US_West2,
            Amazon_SA_East1,
            Amazon_AP_SouthEast2,
            Amazon_CN_North1,
            Amazon_EU_Central1,
            Amazon_AP_Northeast2,
            Amazon_AP_South1,
            Amazon_US_East2,
            Amazon_CA_Central1,
            Amazon_EU_West2
        };
    }  
}