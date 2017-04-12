using System;

using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    // A location may be either a region or zone
    public class Location : ILocation, IEquatable<Location>
    {
        private readonly LocationId id;

        public Location(
            ResourceProvider provider,
            ushort regionNumber,
            byte zoneNumber,
            string name,
            LocationFlags flags = 0)
        {
            this.id = new LocationId {
                ProviderId   = provider.Id,
                RegionNumber = regionNumber,
                ZoneNumber   = zoneNumber,
                Flags        = (byte)flags,
            };
            
            Name = name;
        }

        public Location(LocationId id, string name)
        {
            this.id = id;
            Name = name;
        }

        public long Id => id.Value;

        public string Name { get; }

        #region Flags

        public LocationFlags Flags => (LocationFlags)id.Flags;

        public bool IsMultiRegional => (Flags & LocationFlags.MultiRegional) != 0;
        
        #endregion

        #region IResource

        public int ProviderId => id.ProviderId;

        public ResourceType ResourceType
        {
            get => id.ZoneNumber == 0 ? ResourceType.Region : ResourceType.Zone;
        }

        #endregion

        #region Names

        public string RegionName
        {
            get
            {
                return ResourceType == ResourceType.Region
                    ? Name
                    : Name.Substring(0, Name.Length - 1);
            }
        }

        public string ZoneName
        {
            get
            {
                return ResourceType == ResourceType.Zone
                    ? Name.Substring(Name.Length - 1)
                    : null;
            }
        }

        #endregion

        public Location WithZone(char zoneName)
        {
            #region Preconditions
            
            if (ProviderId == ResourceProvider.Microsoft.Id)
            {
                throw new Exception("Azure does not have zones");
            }

            #endregion

            var id = LocationId.Create(Id);

            id.ZoneNumber = ZoneHelper.GetNumber(zoneName);

            // AMAZON: us-east-1a
            // GOOGLE: us-central1-b, us-central1-c

            var name = Name;

            if (ProviderId == ResourceProvider.Google.Id)
            {
                name += "-" + char.ToLower(zoneName);
            }
            else
            {
                name += char.ToLower(zoneName);
            }

            return new Location(id, name);
        }

        #region Equality

        public bool Equals(Location other) =>
            other.Id == Id &&
            other.Name == Name;

        #endregion
    }
}