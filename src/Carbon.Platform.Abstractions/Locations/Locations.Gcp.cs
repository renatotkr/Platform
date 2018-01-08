using System;

using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    public static partial class Locations
    {
        // GCP | https://cloud.google.com/compute/docs/regions-zones/regions-zones
        public static class Gcp
        { 
            // Multi-Regions
            public static readonly Location US             = new Location(LocationId.Create(3, 0, 1), "us"); 
            public static readonly Location EU             = new Location(LocationId.Create(3, 0, 2), "eu"); 
            public static readonly Location Asia           = new Location(LocationId.Create(3, 0, 3), "asia");

            // Regions                                                                                              
            public static readonly Location US_Central1          = Create(1,  "us-central1");          // | US | Iowa          | ?
            public static readonly Location Europe_West1         = Create(2,  "europe-west1");         // | EU | Belgium       | ?
            public static readonly Location Asia_East1           = Create(3,  "asia-east1");           // | AP | Taiwan        | 2014-04-07
            public static readonly Location US_East1             = Create(4,  "us-east1");             // | US | South Caroli. | 2015-10-01
            public static readonly Location US_West1             = Create(5,  "us-west1");             // | US | Oregon        | 2016-06-20 
            public static readonly Location Asia_NorthEast1      = Create(6,  "asia-northeast1");      // | AP | Tokyo         | 2016-11-08
            public static readonly Location Asia_SouthEast1      = Create(7,  "asia-southeast1");      // | AP | Singapore     | 2017-04-??
            public static readonly Location US_East4             = Create(8,  "us-east4");             // | US | N. Virgina    | 2017-05-10
            public static readonly Location Australia_SouthEast1 = Create(9,  "australia-southeast1"); // | AU | Sydney        | 2017-06-20
            public static readonly Location Europe_West2         = Create(10, "europe-west2");         // | GB | London        | 2017-07-13
            public static readonly Location Europe_West3         = Create(11, "europe-west3");         // | DE | Frankfurt     | 2017-09-12
            public static readonly Location SouthAmerica_East1   = Create(12, "southamerica-east1");   // | BR | São Paulo     | 2017-09-19
            public static readonly Location Asia_South1          = Create(13, "asia-south1");          // | IN | Mumbai        | 2017-10-31
            
            public static Location Get(string name)
            {
                if (name == null)
                    throw new ArgumentNullException(nameof(name));

                var lastChar = name[name.Length - 1];

                // -(a|b|c|d|...)
                if (!char.IsDigit(lastChar))
                {
                    var region = Get(name.Substring(0, name.Length - 2));

                    return region.WithZone(lastChar);
                }

                // Check if it's a zone
                foreach (var location in All)
                {
                    if (location.Name == name) return location;
                }

                throw ResourceError.NotFound(ResourceProvider.Gcp, ResourceTypes.Location, name);

            }

            private static Location Create(ushort regionNumber, string name)
            {
                return new Location(LocationId.Create(ResourceProvider.Gcp, regionNumber), name);
            }

            public static Location FindByRegionNumber(int regionNumber)
            {
                switch (regionNumber)
                {
                    case 01 : return US_Central1;
                    case 02 : return Europe_West1;
                    case 03 : return Asia_East1;
                    case 04 : return US_East1;
                    case 05 : return US_West1;
                    case 06 : return Asia_NorthEast1;
                    case 07 : return Asia_NorthEast1;
                    case 08 : return Asia_SouthEast1;
                    case 09 : return US_East4;
                    case 10 : return Europe_West2;
                    case 11 : return Europe_West3;
                    case 12 : return SouthAmerica_East1;
                    case 13 : return Asia_South1;
                }

                throw ResourceError.NotFound(ResourceProvider.Gcp, ResourceTypes.Location, "region#" + regionNumber);
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
    }  
}

/*
Los Angeles (United States)
Finland
Montreal (Canada)
Netherlands
*/

// ref: https://cloud.google.com/compute/docs/regions-zones/regions-zones
