using System;

namespace Carbon.Platform
{
    // A location may a multi-region, region, or zone

    public class Location : ILocation, IEquatable<Location>
    {
        private readonly LocationId id;

        public Location(LocationId id, string name)
        {
            this.id = id;
            Name = name;
        }

        public int Id => id.Value;

        public string Name { get; }

        #region Helpers

        public int ProviderId => id.ProviderId;

        public LocationType Type => id.Type;

        #endregion

        #region Names

        public string RegionName
        {
            get => Type == LocationType.Zone
                ? Name.Substring(0, Name.Length - 1)
                : Name;
        }

        public string ZoneName
        {
            get => Type == LocationType.Zone
                ? Name.Substring(Name.Length - 1)
                : null;
        }

        #endregion

        #region Helpers

        public Location WithZone(char zoneName)
        {
            #region Preconditions
            
            if (this.id.ProviderId == ResourceProvider.Microsoft.Id)
            {
                throw new Exception("Azure does not have zones");
            }

            #endregion

            var zoneNumber = ZoneHelper.GetNumber(zoneName);

            var newId = LocationId.Create(Id).WithZoneNumber(zoneNumber);

            // AMAZON: us-east-1a
            // GOOGLE: us-central1-b, us-central1-c

            var name = Name;

            if (newId.ProviderId == ResourceProvider.Gcp.Id)
            {
                name += "-" + char.ToLower(zoneName);
            }
            else
            {
                name += char.ToLower(zoneName);
            }

            return new Location(newId, name);
        }

        #endregion

        #region Equality

        public bool Equals(Location other) =>
            other.Id == Id &&
            other.Name == Name;

        #endregion
    }
}