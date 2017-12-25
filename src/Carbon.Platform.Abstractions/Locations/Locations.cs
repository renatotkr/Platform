using System;

using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    public static partial class Locations
    {
        public static readonly Location Global = new Location(0, "global");

        public static Location Get(ResourceProvider provider, string name)
        {
            #region Preconditions

            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            switch (provider.Id)
            {
                case 2   : return Aws.Get(name); 
                case 3   : return Gcp.Get(name);
                case 10  : return DigitalOcean.Get(name);
                case 105 : return GCore.Get(name);
            }
            
            throw ResourceError.NotFound(provider, ResourceTypes.Location, name);
        }

        public static Location Get(int id)
        {
            var lid = LocationId.Create(id);

            if (lid.ProviderId == 2) // AWS
            {
                var region = Aws.FindByRegionNumber(lid.RegionNumber);

                return (lid.ZoneNumber != 0)
                    ? region.WithZone(ZoneHelper.GetLetter(lid.ZoneNumber))
                    : region;
            }

            if (lid.ProviderId == 3) // GCP
            {
                if (lid.Type == LocationType.MultiRegion)
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

            if (lid.ProviderId == 4) // Azure
            {
                throw new Exception("Azure is not supported");
            }

            if (lid.ProviderId == ResourceProvider.GCore.Id)
            {
                return GCore.Get(id);
            }

            if (lid.ProviderId == 10) // Digital Ocean
            {
                return DigitalOcean.FindByRegionNumber(lid.RegionNumber);
            }

            if (lid.ProviderId == ResourceProvider.Wasabi.Id)
            {
                return Wasabi.FindByRegionNumber(lid.RegionNumber);
            }

            throw new Exception($"Unexpected id: {lid.ProviderId}|{lid.RegionNumber}|{lid.ZoneNumber}");
        }
    }  
}