using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    public static partial class Locations
    {
        public static class Vultr
        {
            public static readonly Location EWR = Create(01, "EWR"); // ? | US/New Jersey
            public static readonly Location ORD = Create(02, "ORD"); // ? | US/IL/Chicago
            public static readonly Location DFW = Create(03, "DFW"); // ? | Dallas
            public static readonly Location SEA = Create(04, "SEA"); // ? | Seattle
            public static readonly Location LAX = Create(05, "LAX"); // ? | Los Angeles
            public static readonly Location ATL = Create(06, "ATL"); // ? | Atlanta
            public static readonly Location AMS = Create(07, "AMS"); // ? | Amsterdam
            public static readonly Location LHR = Create(08, "LHR"); // ? | London
            public static readonly Location FRA = Create(09, "FRA"); // ? | Frankfurt
            public static readonly Location SJC = Create(10, "SJC"); // ? | Silicon Valley
            public static readonly Location SYD = Create(11, "SYD"); // ? | AU/Sydney
            public static readonly Location CDG = Create(12, "CDG"); // ? | Paris
            public static readonly Location NRT = Create(13, "NRT"); // ? | JP/Tokyo
            public static readonly Location MIA = Create(14, "MIA"); // ? | US/Miami
            public static readonly Location SGP = Create(15, "SGP"); // ? | Singapore

            public static Location Get(string name)
            {
                switch (name.ToUpper())
                {
                    case "EWR": return EWR;
                    case "ORD": return ORD;
                    case "DFW": return DFW;
                    case "SEA": return SEA;
                    case "LAX": return LAX;
                    case "ATL": return ATL;
                    case "AMS": return AMS;
                    case "LHR": return LHR;
                    case "FRA": return FRA;
                    case "SJC": return SJC;
                    case "SYD": return SYD;
                    case "CDG": return CDG;
                    case "NRT": return NRT;
                    case "MIA": return MIA;
                    case "SGP": return SGP;
                }

                throw ResourceError.NotFound(ResourceProvider.Vultr, ResourceTypes.Location, name);
            }

            public static Location FindByRegionNumber(int regionNumber)
            {
                switch (regionNumber)
                {
                    case 01: return EWR;
                    case 02: return ORD;
                    case 03: return DFW;
                    case 04: return SEA;
                    case 05: return LAX;
                    case 06: return ATL;
                    case 07: return AMS;
                    case 08: return LHR;
                    case 09: return FRA;
                    case 10: return SJC;
                    case 11: return SYD;
                    case 12: return CDG;
                    case 13: return NRT;
                    case 14: return MIA;
                    case 15: return SGP;
                }

                throw ResourceError.NotFound(ResourceProvider.Vultr, ResourceTypes.Location, "region#" + regionNumber);
            }

            private static Location Create(ushort regionNumber, string name)
            {
                return new Location(LocationId.Create(ResourceProvider.Vultr, regionNumber), name);
            }
        }
    }
}

// https://api.vultr.com/v1/regions/list

