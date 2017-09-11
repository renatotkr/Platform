using System;

using Carbon.Extensions;
using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    using static ResourceProvider;

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

            throw ResourceError.NotFound(provider, ResourceTypes.Location, name);
        }

        private static Location GetAwsRegionByNumber(int number)
        {
            switch (number)
            {
                case 1  : return Aws_USEast1;
                case 2  : return Aws_EUWest1;
                case 3  : return Aws_USWest1;
                case 4  : return Aws_APSouthEast1;
                case 5  : return Aws_APNorthEast1;
                case 6  : return Aws_USGovWest1;
                case 7  : return Aws_USWest2;
                case 8  : return Aws_SAEast1;
                case 9  : return Aws_APSouthEast2;
                case 10 : return Aws_CNNorth1;
                case 11 : return Aws_EUCentral1;
                case 12 : return Aws_APNortheast2;
                case 13 : return Aws_APSouth1;
                case 14 : return Aws_USEast2;
                case 15 : return Aws_CACentral1;
                case 16 : return Aws_EUWest2;
                default : throw new Exception("Unknown AWS REGION #" + number);
            }
        }

        public static Location Get(int id)
        {
            var lid = LocationId.Create(id);

            if (lid.ProviderId == Aws.Id)
            {
                var region = GetAwsRegionByNumber(lid.RegionNumber);

                if (lid.ZoneNumber != 0)
                {
                    return region.WithZone(ZoneHelper.GetLetter(lid.ZoneNumber));
                }
                else
                {
                    return region;
                }
            }

            if (lid.ProviderId == Azure.Id) // Azure
            {
                throw new Exception("Azure is not supported");
            }
            
            if (lid.ProviderId == Gcp.Id) // GCP
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


            if (lid.ProviderId == DigitalOcean.Id)
            {
                switch (lid.RegionNumber)
                {
                    case 1: return DigitalOcean_NYC1;
                    case 7: return DigitalOcean_NYC3;
                }
            }

            if (lid.ProviderId == Wasabi.Id)
            {
                switch (lid.RegionNumber)
                {
                    case 1: return Wasabi_USEast1;
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

        public static readonly Location Aws_USEast1      = Create(Aws,  1, "us-east-1");       // | US    | N. Virginia   | 2006-08-25
        public static readonly Location Aws_EUWest1      = Create(Aws,  2, "eu-west-1");       // | EU    | Ireland       | 2008-12-10
        public static readonly Location Aws_USWest1      = Create(Aws,  3, "us-west-1");       // | US    | N. California | 2009-12-03
        public static readonly Location Aws_APSouthEast1 = Create(Aws,  4, "ap-southeast-1");  // | AP    | Singapore     | 2010-04-29
        public static readonly Location Aws_APNorthEast1 = Create(Aws,  5, "ap-northeast-1");  // | AP    | Tokyo         | 2011-03-02
        public static readonly Location Aws_USGovWest1   = Create(Aws,  6, "us-gov-west-1");   // | US    | AWS GovCloud  | 2011-08-16
        public static readonly Location Aws_USWest2      = Create(Aws,  7, "us-west-2");       // | US    | Oregon        | 2011-11-09
        public static readonly Location Aws_SAEast1      = Create(Aws,  8, "sa-east-1");       // | SA    | São Paulo     | 2011-12-14
        public static readonly Location Aws_APSouthEast2 = Create(Aws,  9, "ap-southeast-2");  // | AP    | Sydney        | 2012-11-12
        public static readonly Location Aws_CNNorth1     = Create(Aws, 10, "cn-north-1");      // | China | Beijing       | 2013-12-18
        public static readonly Location Aws_EUCentral1   = Create(Aws, 11, "eu-central-1");    // | EU    | Frankfurt     | 2014-10-23
        public static readonly Location Aws_APNortheast2 = Create(Aws, 12, "ap-northeast-2");  // | AP    | Seoul	      | 2016-01-06
        public static readonly Location Aws_APSouth1     = Create(Aws, 13, "ap-south-1");      // | AP    | Mumbai        | 2016-06-27
        public static readonly Location Aws_USEast2      = Create(Aws, 14, "us-east-2");       // | US    | Ohio          | 2016-10-17
        public static readonly Location Aws_CACentral1   = Create(Aws, 15, "ca-central-1");    // | CA    | Central       | 2016-12-08
        public static readonly Location Aws_EUWest2      = Create(Aws, 16, "eu-west-2");       // | EU    | London        | 2016-12-13

        /*
        Paris
        Ningxia
        Stockholm
        Hong Kong
        */

        // Azure (https://azure.microsoft.com/en-us/regions/) -- announced 2008, launched 2010-01
        // ------------------------------------------------------------------------------------------------------------------------------------------

        // Not settled on these ids yet... but close.

        public static readonly Location Azure_US_NorthCentral = Create(Azure,  1, "North Central US");   // | US/IL       | ~ 2010
        public static readonly Location Azure_US_SouthCentral = Create(Azure,  2, "South Central US");   // | US/TX       | ~ 2010
        public static readonly Location Azure_EU_North        = Create(Azure,  3, "North Europe");       // | Ireland     | ?
        public static readonly Location Azure_EU_West         = Create(Azure,  4, "West Europe");        // | Netherlands | ? 
        public static readonly Location Azure_AP_East         = Create(Azure,  5, "East Asia");          // | Hong Kong   | ? < 2010-05-01
        public static readonly Location Azure_AP_SouthEast    = Create(Azure,  6, "Southeast Asia");     // | Singapore   | ? < 2010-05-01 
        public static readonly Location Azure_US_East         = Create(Azure,  7, "East US");            // | US/Virginia | ? < 2011-06-06
        public static readonly Location Azure_US_West         = Create(Azure,  8, "West US");            // | US/CA       | ? < 2011-06-06
        public static readonly Location Azure_JP_East         = Create(Azure,  9, "Japan East");         // | Saitama     | 2014-02-25 GA
        public static readonly Location Azure_JP_West         = Create(Azure, 10, "Japan West");         // | Osaka       | 2014-02-25 GA    
        
        public static readonly Location Azure_CN_East         = Create(Azure, 11, "China East");          // | Shanghai    | 2014-03-27
        public static readonly Location Azure_CN_North        = Create(Azure, 12, "China North");         // | Beijing     | 2014-03-27      
        public static readonly Location Azure_BR_South        = Create(Azure, 13, "Brazil South");        // | Sao Paulo   | 2014-04-17 PV 
        public static readonly Location Azure_US_East2        = Create(Azure, 14, "East US 2");           // | US/Virginia | 2014-07-09 ?
        public static readonly Location Azure_US_WestCentral  = Create(Azure, 15, "West Central US");     // | US/?
        public static readonly Location Azure_US_Central      = Create(Azure, 16, "Central US");          // | US/IA       | 2014-07-09 ?
        public static readonly Location Azure_AU_East	      = Create(Azure, 17, "Australia East");      // | AU/NSW      | 2014-10-27 GA
        public static readonly Location Azure_AU_SouthEast    = Create(Azure, 18, "Australia Southeast"); // | Victoria    | 2014-10-27 GA
        public static readonly Location Azure_US_GOV_Virgina  = Create(Azure, 19, "US Gov Virginia");     // | US/Virginia | 2014-12-09 GA 
        public static readonly Location Azure_US_GOV_Iowa     = Create(Azure, 20, "US Gov Iowa");         // | US/Iowa     | 2014-12-09 GA
        public static readonly Location Azure_IN_Central	  = Create(Azure, 21, "Central India");       // | Pune        | 2015-09-29 ?
        public static readonly Location Azure_IN_West	      = Create(Azure, 22, "West India");          // | Mumbai      | 2015-09-29 ?
        public static readonly Location Azure_IN_South	      = Create(Azure, 23, "South India");         // | Chennai     | 2015-09-29 ?
        public static readonly Location Azure_DE_Central      = Create(Azure, 24, "Germany Central");     // | Frankfurt   | 2016-03-15
        public static readonly Location Azure_DE_NorthEast    = Create(Azure, 25, "Germany Northeast");   // | Magdeburg   | 2016-03-15  
        public static readonly Location Azure_CA_East         = Create(Azure, 26, "Canada East");         // | Quebec City | 2016-05-10 GA
        public static readonly Location Azure_CA_Central      = Create(Azure, 27, "Canada Central");      // | Toronto     | 2016-05-10 GA
        public static readonly Location Azure_US_West2        = Create(Azure, 28, "West US 2");           // | US          | 2016-07-13 GA
        public static readonly Location Azure_UK_West         = Create(Azure, 29, "UK West");             // | Cardiff     | 2016-09-07 GA
        public static readonly Location Azure_UK_South        = Create(Azure, 30, "UK South");            // | London      | 2016-09-07 GA
        public static readonly Location Azure_US_DoD_East     = Create(Azure, 31, "US DoD East");         // | US/?        | 2016-10-18 ?
        public static readonly Location Azure_US_DoD_Central  = Create(Azure, 32, "US DoD Central");      // | US?         | 2016-10-18 ?
        public static readonly Location Azure_KR_Central      = Create(Azure, 33, "Korea Central");       // | Seoul       | 2017-02-21 GA
        public static readonly Location Azure_KR_South        = Create(Azure, 34, "Korea South");         // | Busan       | 2017-02-21 GA

        // As of Feb 28th, 2016

        // Brazil South : 2014-06-05 GA   
        // US Central   : 2014-07-14 GA
        // US East 2    : 2014-07-14 GA

        // SOON
        public static readonly Location Azure_FR_Central     = Create(Azure, 35, "France Central");        // | 2016-10-03
        public static readonly Location Azure_FR_South       = Create(Azure, 36, "France South");          // | 2016-10-03
        public static readonly Location Azure_US_GOV_AZ      = Create(Azure, 37, "US Gov Arizona");
        public static readonly Location Azure_US_GOV_TT      = Create(Azure, 38, "US Gov Texas");


        // GCP | https://cloud.google.com/compute/docs/regions-zones/regions-zones
        // ------------------------------------------------------------------------------------------------------------------------------------------

        // Multi-Regions
        public static readonly Location Gcp_US             = new Location(LocationId.Create(Gcp, 0, 1), "us"); 
        public static readonly Location Gcp_EU             = new Location(LocationId.Create(Gcp, 0, 2), "eu"); 
        public static readonly Location Gcp_Asia           = new Location(LocationId.Create(Gcp, 0, 3), "asia");

        // Regions                                                                                                  
        public static readonly Location Gcp_USCentral1     = Create(Gcp, 1, "us-central1");     // | US    | Iowa          | ?
        public static readonly Location Gcp_EuropeWest1    = Create(Gcp, 2, "europe-west1");    // | EU    | Belgium       | ?
        public static readonly Location Gcp_AsiaEast1      = Create(Gcp, 3, "asia-east1");      // | AP    | Taiwan        | 2014-04-07
        public static readonly Location Gcp_USEast1        = Create(Gcp, 4, "us-east1");        // | US    | South Caroli. | 2015-10-01
        public static readonly Location Gcp_USWest1        = Create(Gcp, 5, "us-west1");        // | US    | Oregon        | 2016-06-20 
        public static readonly Location Gcp_AsiaNorthEast1 = Create(Gcp, 6, "asia-northeast1"); // | AP    | Tokyo         | 2016-11-08
        public static readonly Location Gcp_AsiaSouthEast1 = Create(Gcp, 7, "asia-southeast1"); // | AP    | Singapore     | 2017-04-??
        public static readonly Location Gcp_USEast4        = Create(Gcp, 8, "us-east4");        // | US    | N. Virgina    | 2017-05-10


        // Digital Ocean
        // ------------------------------------------------------------------------------------------------------------------------------------------

        public static readonly Location DigitalOcean_NYC1 = Create(DigitalOcean, 1,  "NYC1");  // < 2012
        public static readonly Location DigitalOcean_SFO1 = Create(DigitalOcean, 2,  "SFO1"); // ??
        public static readonly Location DigitalOcean_NYC2 = Create(DigitalOcean, 3,  "NYC2"); // 2013-07-30 // Google building
        public static readonly Location DigitalOcean_AMS2 = Create(DigitalOcean, 4,  "AMS2"); // 2013-12-02 | Amsterdam
        public static readonly Location DigitalOcean_SGP1 = Create(DigitalOcean, 5,  "SGP1"); // 2014-02-11 | Singapore
        public static readonly Location DigitalOcean_LON1 = Create(DigitalOcean, 6,  "LON1"); // 2014-07-15 | London
        public static readonly Location DigitalOcean_NYC3 = Create(DigitalOcean, 7,  "NYC3"); // 2014-08-19 | NYC
        public static readonly Location DigitalOcean_AMS3 = Create(DigitalOcean, 8,  "AMS3"); // 2014-10-11 | Amsterdam
        public static readonly Location DigitalOcean_FRA1 = Create(DigitalOcean, 9,  "FRA1"); // 2015-04-15 | Frankfurt
        public static readonly Location DigitalOcean_TOR1 = Create(DigitalOcean, 10, "TOR1"); // 2015-10-23 | Toronto, CA
        public static readonly Location DigitalOcean_BLR1 = Create(DigitalOcean, 11, "BLR1"); // 2016-05-31 | Bangalore, IN
        public static readonly Location DigitalOcean_SFO2 = Create(DigitalOcean, 12, "SFO2"); // 2017-07-12 | San Francisco


        // Wasabai

        public static readonly Location Wasabi_USEast1 = Create(Wasabi, 1, "us-east-1");

        public static readonly Location[] AwsRegions = new[] {
            Aws_USEast1,
            Aws_EUWest1,
            Aws_USWest1,
            Aws_APSouthEast1,
            Aws_APNorthEast1,
            Aws_USGovWest1,
            Aws_USWest2,
            Aws_SAEast1,
            Aws_APSouthEast2,
            Aws_CNNorth1,
            Aws_EUCentral1,
            Aws_APNortheast2,
            Aws_APSouth1,
            Aws_USEast2,
            Aws_CACentral1,
            Aws_EUWest2
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