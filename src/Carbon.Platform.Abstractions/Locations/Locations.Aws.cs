using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    public static partial class Locations
    {
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
                }

                throw ResourceError.NotFound(ResourceProvider.Aws, ResourceTypes.Location, "region#" + regionNumber);
            }

            public static Location Get(string name)
            {
                // ap-northeast-2a
                
                var lastCharacter = name[name.Length - 1];

                // it's a zone if it ends with a character
                if (char.IsLetter(lastCharacter)) 
                {
                    var region = Get(name.Substring(0, name.Length - 1));

                    return region.WithZone(lastCharacter);
                }

                foreach (var region in All)
                {
                    if (region.Name == name) return region;
                }

                throw ResourceError.NotFound(ResourceProvider.Aws, ResourceTypes.Location, name);
            }

            private static Location Create(ushort regionNumber, string name)
            {
                return new Location(LocationId.Create(ResourceProvider.Aws, regionNumber), name);
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
    }

    // SOON
    /*
    Ningxia
    Stockholm
    Hong Kong
    */
}