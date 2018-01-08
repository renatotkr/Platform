namespace Carbon.Platform
{
    public static partial class Locations
    {
        // Azure (https://azure.microsoft.com/en-us/regions/) 
        // - announced 2008
        // - launched  2010-01

        public static class Azure
        {
            public static readonly Location USNorthCentral = Create(01, "North Central US");    // | US | IL       | ~ 2010
            public static readonly Location USSouthCentral = Create(02, "South Central US");    // | US | TX       | ~ 2010
            public static readonly Location EUNorth        = Create(03, "North Europe");        // | Ireland       | ?
            public static readonly Location EUWest         = Create(04, "West Europe");         // | Netherlands   | ? 
            public static readonly Location APEast         = Create(05, "East Asia");           // | Hong Kong     | ? < 2010-05-01
            public static readonly Location APSouthEast    = Create(06, "Southeast Asia");      // | Singapore     | ? < 2010-05-01 
            public static readonly Location USEast         = Create(07, "East US");             // | US | Virginia | ? < 2011-06-06
            public static readonly Location USWest         = Create(08, "West US");             // | US | CA       | ? < 2011-06-06
            public static readonly Location JPEast         = Create(09, "Japan East");          // | Saitama       | 2014-02-25 GA
            public static readonly Location JPWest         = Create(10, "Japan West");          // | Osaka         | 2014-02-25 GA    
            public static readonly Location CNEast         = Create(11, "China East");          // | Shanghai      | 2014-03-27
            public static readonly Location CNNorth        = Create(12, "China North");         // | Beijing       | 2014-03-27      
            public static readonly Location BRSouth        = Create(13, "Brazil South");        // | Sao Paulo     | 2014-04-17 PV, 2014-07-14 GA 
            public static readonly Location USEast2        = Create(14, "East US 2");           // | US/Virginia   | 2014-07-09 ? , 2014-07-14 GA
            public static readonly Location USCentral      = Create(15, "Central US");          // | US | IA       | 2014-07-09 ? , 2014-07-14 GA
            public static readonly Location AUEast	       = Create(16, "Australia East");      // | AU/NSW        | 2014-10-27 GA
            public static readonly Location AUSouthEast    = Create(17, "Australia Southeast"); // | Victoria      | 2014-10-27 GA
            public static readonly Location USGOV_Virgina  = Create(18, "US Gov Virginia");     // | US | Virginia | 2014-12-09 GA 
            public static readonly Location USGOV_Iowa     = Create(19, "US Gov Iowa");         // | US | Iowa     | 2014-12-09 GA
            public static readonly Location INCentral	   = Create(20, "Central India");       // | Pune          | 2015-09-29 ?
            public static readonly Location INWest	       = Create(21, "West India");          // | Mumbai        | 2015-09-29 ?
            public static readonly Location INSouth	       = Create(22, "South India");         // | Chennai       | 2015-09-29 ?
            public static readonly Location DECentral      = Create(23, "Germany Central");     // | Frankfurt     | 2016-03-15
            public static readonly Location DENorthEast    = Create(24, "Germany Northeast");   // | Magdeburg     | 2016-03-15  
            public static readonly Location CAEast         = Create(25, "Canada East");         // | Quebec City   | 2016-05-10 GA
            public static readonly Location CACentral      = Create(26, "Canada Central");      // | Toronto       | 2016-05-10 GA
            public static readonly Location USWestCentral  = Create(27, "West Central US");     // | US | ?        | 2016-06-13 GA
            public static readonly Location USWest2        = Create(28, "West US 2");           // | US            | 2016-06-13 GA
            public static readonly Location UKWest         = Create(29, "UK West");             // | Cardiff       | 2016-09-07 GA
            public static readonly Location UKSouth        = Create(30, "UK South");            // | London        | 2016-09-07 GA
            public static readonly Location USDoD_East     = Create(31, "US DoD East");         // | US | ?        | 2016-10-18 ?
            public static readonly Location USDoD_Central  = Create(32, "US DoD Central");      // | US | ?        | 2016-10-18 ?
            public static readonly Location KRCentral      = Create(33, "Korea Central");       // | Seoul         | 2017-02-21 GA
            public static readonly Location KRSouth        = Create(34, "Korea South");         // | Busan         | 2017-02-21 GA

            // As of Feb 28th, 2016 ---

            // SOON
            public static readonly Location FRCentral      = Create(35, "France Central");        // | 2016-10-03
            public static readonly Location FRSouth        = Create(36, "France South");          // | 2016-10-03
            public static readonly Location USGovArizona   = Create(37, "US Gov Arizona");
            public static readonly Location USGovTexas     = Create(38, "US Gov Texas");
                                                       
            // South Africa West
            // South Africa North
            // Australia Central

            public static Location Create(ushort regionNumber, string name)
            {
                return new Location(LocationId.Create(ResourceProvider.Azure, regionNumber), name);
            }
        }
    }  
}