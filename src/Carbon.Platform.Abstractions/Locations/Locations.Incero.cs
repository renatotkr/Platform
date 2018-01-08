using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    public static partial class Locations
    {
        public static class Incero
        {
            public static readonly Location DAL = Create(1, "DAL"); // ? | US/TX/Dallas
            public static readonly Location NYC = Create(2, "NYC"); // ? | US/NY/New`York
            public static readonly Location SEA = Create(3, "SEA"); // ? | US/WA/Seattle
           
            public static Location Get(string name)
            {
                switch (name.ToUpper())
                {
                    case "DAL": return DAL;
                    case "NYC": return NYC;
                    case "SEA": return SEA;
                }

                throw ResourceError.NotFound(ResourceProvider.Incero, ResourceTypes.Location, name);
            }

            public static Location FindByRegionNumber(int regionNumber)
            {
                switch (regionNumber)
                {
                    case 01: return DAL;
                    case 02: return NYC;
                    case 03: return SEA;
                }

                throw ResourceError.NotFound(ResourceProvider.Incero, ResourceTypes.Location, "region#" + regionNumber);
            }

            private static Location Create(ushort regionNumber, string name)
            {
                return new Location(LocationId.Create(ResourceProvider.Vultr, regionNumber), name);
            }
        }
    }
}