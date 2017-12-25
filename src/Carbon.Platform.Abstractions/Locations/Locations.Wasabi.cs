using System;
using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    public static partial class Locations
    {
        public static class Wasabi
        {
            public static readonly Location USEast1 = Create(1, "us-east-1");
            
            public static Location FindByRegionNumber(int regionNumber)
            {
                switch (regionNumber)
                {
                    case 1  : return USEast1;
                }

                throw ResourceError.NotFound(ResourceProvider.Wasabi, ResourceTypes.Location, "region#" + regionNumber);

            }

            private static Location Create(ushort regionNumber, string name)
            {
                return new Location(LocationId.Create(ResourceProvider.Wasabi, regionNumber), name);
            }
        }
    }  
}