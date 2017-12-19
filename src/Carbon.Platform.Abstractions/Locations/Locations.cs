using System;

using Carbon.Extensions;
using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    public static class Locations
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

            foreach (var region in Aws.All)
            {
                if (region.Name == name) return region;
            }

            throw ResourceError.NotFound(provider, ResourceTypes.Location, name);
        }

        public static Location Get(int id)
        {
            var lid = LocationId.Create(id);

            if (lid.ProviderId == ResourceProvider.Aws.Id) // 2
            {
                var region = Aws.FindByRegionNumber(lid.RegionNumber);

                if (lid.ZoneNumber != 0)
                {
                    return region.WithZone(ZoneHelper.GetLetter(lid.ZoneNumber));
                }
                else
                {
                    return region;
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
                        case 1  : return Gcp.US;
                        case 2  : return Gcp.EU;
                        case 3  : return Gcp.Asia;
                        default : throw new Exception("No multi-region found:" + lid.ZoneNumber);
                    }
                }

                return Gcp.FindByRegionNumber(lid.RegionNumber);
            }

            if (lid.ProviderId == ResourceProvider.DigitalOcean.Id)
            {
                return DigitalOcean.FindByRegionNumber(lid.RegionNumber);
            }

            if (lid.ProviderId == ResourceProvider.Wasabi.Id)
            {
                return Wasabi.FindByRegionNumber(lid.RegionNumber);
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

        public static class Aws
        {
            public static readonly Location USEast1      = Create(1, "us-east-1");       // | US    | N. Virginia   | 2006-08-25
            public static readonly Location EUWest1      = Create(2, "eu-west-1");       // | EU    | Ireland       | 2008-12-10
            public static readonly Location USWest1      = Create(3, "us-west-1");       // | US    | N. California | 2009-12-03
            public static readonly Location APSouthEast1 = Create(4, "ap-southeast-1");  // | AP    | Singapore     | 2010-04-29
            public static readonly Location APNorthEast1 = Create(5, "ap-northeast-1");  // | AP    | Tokyo         | 2011-03-02
            public static readonly Location USGovWest1   = Create(6, "us-gov-west-1");   // | US    | AWS GovCloud  | 2011-08-16
            public static readonly Location USWest2      = Create(7, "us-west-2");       // | US    | Oregon        | 2011-11-09
            public static readonly Location SAEast1      = Create(8, "sa-east-1");       // | SA    | São Paulo     | 2011-12-14
            public static readonly Location APSouthEast2 = Create(9, "ap-southeast-2");  // | AP    | Sydney        | 2012-11-12
            public static readonly Location CNNorth1     = Create(10, "cn-north-1");     // | China | Beijing       | 2013-12-18
            public static readonly Location EUCentral1   = Create(11, "eu-central-1");   // | EU    | Frankfurt     | 2014-10-23
            public static readonly Location APNortheast2 = Create(12, "ap-northeast-2"); // | AP    | Seoul	        | 2016-01-06
            public static readonly Location APSouth1     = Create(13, "ap-south-1");     // | AP    | Mumbai        | 2016-06-27
            public static readonly Location USEast2      = Create(14, "us-east-2");      // | US    | Ohio          | 2016-10-17
            public static readonly Location CACentral1   = Create(15, "ca-central-1");   // | CA    | Central       | 2016-12-08
            public static readonly Location EUWest2      = Create(16, "eu-west-2");      // | EU    | London        | 2016-12-13
            public static readonly Location CNNorthwest1 = Create(17, "cn-northwest-1"); // | CN    | Ningxia       | 2017-12-11
            public static readonly Location EUWest3      = Create(18, "eu-west-3");      // | EU    | Paris         | 2018-12-18

            public static Location Create(ushort regionNumber, string name)
            {
                return new Location(LocationId.Create(ResourceProvider.Aws, regionNumber), name);
            }

            internal static Location FindByRegionNumber(int regionNumber)
            {
                switch (regionNumber)
                {
                    case 1  : return USEast1;
                    case 2  : return EUWest1;
                    case 3  : return USWest1;
                    case 4  : return APSouthEast1;
                    case 5  : return APNorthEast1;
                    case 6  : return USGovWest1;
                    case 7  : return USWest2;
                    case 8  : return SAEast1;
                    case 9  : return APSouthEast2;
                    case 10 : return CNNorth1;
                    case 11 : return EUCentral1;
                    case 12 : return APNortheast2;
                    case 13 : return APSouth1;
                    case 14 : return USEast2;
                    case 15 : return CACentral1;
                    case 16 : return EUWest2;
                    case 17 : return CNNorthwest1;
                    case 18 : return EUWest3;
                    default : throw new Exception("Unknown AWS region#" + regionNumber);
                }
            }

            public static readonly Location[] All = new[] {
                USEast1,
                EUWest1,
                USWest1,
                APSouthEast1,
                APNorthEast1,
                USGovWest1,
                USWest2,
                SAEast1,
                APSouthEast2,
                CNNorth1,
                EUCentral1,
                APNortheast2,
                APSouth1,
                USEast2,
                CACentral1,
                EUWest2,
                CNNorthwest1,
                EUWest3
            };
        }

        /*
        Paris
        Ningxia
        Stockholm
        Hong Kong
        */


        // Azure (https://azure.microsoft.com/en-us/regions/) -- announced 2008, launched 2010-01
        // ------------------------------------------------------------------------------------------------------------------------------------------
        public static class Azure
        {
            public static readonly Location US_NorthCentral = Create( 1, "North Central US");    // | US | IL       | ~ 2010
            public static readonly Location US_SouthCentral = Create( 2, "South Central US");    // | US | TX       | ~ 2010
            public static readonly Location EU_North        = Create( 3, "North Europe");        // | Ireland       | ?
            public static readonly Location EU_West         = Create( 4, "West Europe");         // | Netherlands   | ? 
            public static readonly Location AP_East         = Create( 5, "East Asia");           // | Hong Kong     | ? < 2010-05-01
            public static readonly Location AP_SouthEast    = Create( 6, "Southeast Asia");      // | Singapore     | ? < 2010-05-01 
            public static readonly Location US_East         = Create( 7, "East US");             // | US | Virginia | ? < 2011-06-06
            public static readonly Location US_West         = Create( 8, "West US");             // | US | CA       | ? < 2011-06-06
            public static readonly Location JP_East         = Create( 9, "Japan East");          // | Saitama       | 2014-02-25 GA
            public static readonly Location JP_West         = Create(10, "Japan West");          // | Osaka         | 2014-02-25 GA    
            public static readonly Location CN_East         = Create(11, "China East");          // | Shanghai      | 2014-03-27
            public static readonly Location CN_North        = Create(12, "China North");         // | Beijing       | 2014-03-27      
            public static readonly Location BR_South        = Create(13, "Brazil South");        // | Sao Paulo     | 2014-04-17 PV, 2014-07-14 GA 
            public static readonly Location US_East2        = Create(14, "East US 2");           // | US/Virginia   | 2014-07-09 ?,  2014-07-14 GA
            public static readonly Location US_Central      = Create(15, "Central US");          // | US | IA       | 2014-07-09 ?, 2014-07-14 GA
            public static readonly Location AU_East	        = Create(16, "Australia East");      // | AU/NSW        | 2014-10-27 GA
            public static readonly Location AU_SouthEast    = Create(17, "Australia Southeast"); // | Victoria      | 2014-10-27 GA
            public static readonly Location US_GOV_Virgina  = Create(18, "US Gov Virginia");     // | US | Virginia | 2014-12-09 GA 
            public static readonly Location US_GOV_Iowa     = Create(19, "US Gov Iowa");         // | US | Iowa     | 2014-12-09 GA
            public static readonly Location IN_Central	    = Create(20, "Central India");       // | Pune          | 2015-09-29 ?
            public static readonly Location IN_West	        = Create(21, "West India");          // | Mumbai        | 2015-09-29 ?
            public static readonly Location IN_South	    = Create(22, "South India");         // | Chennai       | 2015-09-29 ?
            public static readonly Location DE_Central      = Create(23, "Germany Central");     // | Frankfurt     | 2016-03-15
            public static readonly Location DE_NorthEast    = Create(24, "Germany Northeast");   // | Magdeburg     | 2016-03-15  
            public static readonly Location CA_East         = Create(25, "Canada East");         // | Quebec City   | 2016-05-10 GA
            public static readonly Location CA_Central      = Create(26, "Canada Central");      // | Toronto       | 2016-05-10 GA
            public static readonly Location US_WestCentral  = Create(27, "West Central US");     // | US | ?        | 2016-06-13 GA
            public static readonly Location US_West2        = Create(28, "West US 2");           // | US            | 2016-06-13 GA
            public static readonly Location UK_West         = Create(29, "UK West");             // | Cardiff       | 2016-09-07 GA
            public static readonly Location UK_South        = Create(30, "UK South");            // | London        | 2016-09-07 GA
            public static readonly Location US_DoD_East     = Create(31, "US DoD East");         // | US | ?        | 2016-10-18 ?
            public static readonly Location US_DoD_Central  = Create(32, "US DoD Central");      // | US | ?        | 2016-10-18 ?
            public static readonly Location KR_Central      = Create(33, "Korea Central");       // | Seoul         | 2017-02-21 GA
            public static readonly Location KR_South        = Create(34, "Korea South");         // | Busan         | 2017-02-21 GA

            // As of Feb 28th, 2016 ---

            // SOON
            public static readonly Location FR_Central      = Create(35, "France Central");        // | 2016-10-03
            public static readonly Location FR_South        = Create(36, "France South");          // | 2016-10-03
            public static readonly Location US_GOV_AZ       = Create(37, "US Gov Arizona");
            public static readonly Location US_GOV_TT       = Create(38, "US Gov Texas");
                                                       
            // South Africa West
            // South Africa North
            public static Location Create(ushort regionNumber, string name)
            {
                return new Location(LocationId.Create(ResourceProvider.Azure, regionNumber), name);
            }
        }

        // GCP | https://cloud.google.com/compute/docs/regions-zones/regions-zones
        // -----------------------------------------------------------------------------------------------------------------
        public static class Gcp
        { 
            // Multi-Regions
            public static readonly Location US             = new Location(LocationId.Create(3, 0, 1), "us"); 
            public static readonly Location EU             = new Location(LocationId.Create(3, 0, 2), "eu"); 
            public static readonly Location Asia           = new Location(LocationId.Create(3, 0, 3), "asia");

            // Regions                                                                                              
            public static readonly Location US_Central1          = Create(1, "us-central1");          // | US | Iowa          | ?
            public static readonly Location Europe_West1         = Create(2, "europe-west1");         // | EU | Belgium       | ?
            public static readonly Location Asia_East1           = Create(3, "asia-east1");           // | AP | Taiwan        | 2014-04-07
            public static readonly Location US_East1             = Create(4, "us-east1");             // | US | South Caroli. | 2015-10-01
            public static readonly Location US_West1             = Create(5, "us-west1");             // | US | Oregon        | 2016-06-20 
            public static readonly Location Asia_NorthEast1      = Create(6, "asia-northeast1");      // | AP | Tokyo         | 2016-11-08
            public static readonly Location Asia_SouthEast1      = Create(7, "asia-southeast1");      // | AP | Singapore     | 2017-04-??
            public static readonly Location US_East4             = Create(8, "us-east4");             // | US | N. Virgina    | 2017-05-10
            public static readonly Location Australia_SouthEast1 = Create(9, "australia-southeast1"); // | AU | Sydney        | 2017-06-20
            public static readonly Location Europe_West2         = Create(10, "europe-west2");        // | GB | London        | 2017-07-13
            public static readonly Location Europe_West3         = Create(11, "europe-west3");        // | DE |               | 2017-09-12
            public static readonly Location SouthAmerica_East1   = Create(12, "southamerica-east1");  // | BR | São Paulo     | 2017-09-19
            public static readonly Location Asia_South1          = Create(13, "asia-south1");         // |                    | 2017-10-31
           
            private static Location Create(ushort regionNumber, string name)
            {
                return new Location(LocationId.Create(ResourceProvider.Gcp, regionNumber), name);
            }

            public static Location FindByRegionNumber(int regionNumber)
            {
                switch (regionNumber)
                {
                    case 1  : return US_Central1;
                    case 2  : return Europe_West1;
                    case 3  : return Asia_East1;
                    case 4  : return US_East1;
                    case 5  : return US_West1;
                    case 6  : return Asia_NorthEast1;
                    case 7  : return Asia_NorthEast1;
                    case 8  : return Asia_SouthEast1;
                    case 9  : return US_East4;
                    case 10 : return Europe_West2;
                    case 11 : return Europe_West3;
                    case 12 : return SouthAmerica_East1;
                    case 13 : return Asia_South1;

                    default: throw new Exception("No GCP region found for region#" + regionNumber);
                }
            }

            public static readonly Location[] All = new[] {
                US_Central1,
                Europe_West1,
                Asia_East1,
                US_East1,
                US_West1,
                Asia_NorthEast1,
                Asia_SouthEast1,
                US_East4,
                Europe_West2,
                Europe_West3,
                SouthAmerica_East1,
                Asia_South1
            };
        }

        // Digital Ocean
        // -------------------------------------------------------------------------------------------------
        public static class DigitalOcean
        {
            public static readonly Location NYC1 = Create(1,  "NYC1"); // < 2012
            public static readonly Location SFO1 = Create(2,  "SFO1"); // ??
            public static readonly Location NYC2 = Create(3,  "NYC2"); // 2013-07-30 | Google building
            public static readonly Location AMS2 = Create(4,  "AMS2"); // 2013-12-02 | Amsterdam
            public static readonly Location SGP1 = Create(5,  "SGP1"); // 2014-02-11 | Singapore
            public static readonly Location LON1 = Create(6,  "LON1"); // 2014-07-15 | London
            public static readonly Location NYC3 = Create(7,  "NYC3"); // 2014-08-19 | NYC
            public static readonly Location AMS3 = Create(8,  "AMS3"); // 2014-10-11 | Amsterdam
            public static readonly Location FRA1 = Create(9,  "FRA1"); // 2015-04-15 | Frankfurt
            public static readonly Location TOR1 = Create(10, "TOR1"); // 2015-10-23 | Toronto, CA
            public static readonly Location BLR1 = Create(11, "BLR1"); // 2016-05-31 | Bangalore, IN
            public static readonly Location SFO2 = Create(12, "SFO2"); // 2017-07-12 | San Francisco

            public static Location FindByRegionNumber(int regionNumber)
            {
                switch (regionNumber)
                {
                    case 1 : return NYC1;
                    case 2 : return SFO1;
                    case 3 : return NYC2;
                    case 4 : return AMS2;
                    case 5 : return SGP1;
                    case 6 : return LON1;
                    case 7 : return NYC3;
                    case 8 : return AMS3;
                    case 9 : return FRA1;
                    case 10: return TOR1;
                    case 11: return BLR1;
                    case 12: return SFO2;

                    default : throw new Exception("No DigitalOcean region#" + regionNumber);
                }
            }

            private static Location Create(ushort regionNumber, string name)
            {
                return new Location(LocationId.Create(ResourceProvider.DigitalOcean, regionNumber), name);
            }
        }

        public static class Wasabi
        {
            public static readonly Location USEast1 = Create(1, "us-east-1");
            
            public static Location FindByRegionNumber(int regionNumber)
            {
                switch (regionNumber)
                {
                    case 1  : return USEast1;
                    default : throw new Exception("No wasabai region#" + regionNumber);
                }
            }

            private static Location Create(ushort regionNumber, string name)
            {
                return new Location(LocationId.Create(ResourceProvider.Wasabi, regionNumber), name);
            }
        }
    }  
}