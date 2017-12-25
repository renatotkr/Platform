using System;
using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    public static partial class Locations
    {
        // Digital Ocean
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

            public static Location Get(string name)
            {
                switch (name.ToUpper())
                {
                    case "NYC1": return NYC1;
                    case "SFO1": return SFO1;
                    case "NYC2": return NYC2;
                    case "AMS2": return AMS2;
                    case "SGP1": return SGP1;
                    case "LON1": return LON1;
                    case "NYC3": return NYC3;
                    case "AMS3": return AMS3;
                    case "FRA1": return FRA1;
                    case "TOR1": return TOR1;
                    case "BLR1": return BLR1;
                    case "SFO2": return SFO2;
                }

                throw ResourceError.NotFound(ResourceProvider.DigitalOcean, ResourceTypes.Location, name);
            }

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
                }

                throw ResourceError.NotFound(ResourceProvider.DigitalOcean, ResourceTypes.Location, "region#" + regionNumber);
            }

            private static Location Create(ushort regionNumber, string name)
            {
                return new Location(LocationId.Create(ResourceProvider.DigitalOcean, regionNumber), name);
            }
        }
    }  
}