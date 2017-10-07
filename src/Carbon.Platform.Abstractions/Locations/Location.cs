using System;
using System.Runtime.Serialization;
using Carbon.Platform.Resources;

namespace Carbon.Platform
{
    // A location may a multi-region area, region, or zone

    [DataContract]
    public class Location : ILocation, IResource, IEquatable<Location>
    {
        public Location(int id, string name)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [DataMember(Name = "id", Order = 1)]
        public int Id { get; }

        [DataMember(Name = "name", Order = 2)]
        public string Name { get; }

        #region Helpers

        public int ProviderId => LocationId.Create(Id).ProviderId;

        public LocationType Type => LocationId.Create(Id).Type;

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
            var zoneNumber = ZoneHelper.GetNumber(zoneName);

            var newId = LocationId.Create(Id).WithZoneNumber(zoneNumber);

            /* FORMAT DETAILS
            --------------------------------------
            aws | us-east-1a
            gcp | us-central1-b, us-central1-c
            ----------------------------------- */

            var newName = Name;

            if (newId.ProviderId == ResourceProvider.Gcp.Id)
            {
                newName += "-" + char.ToLower(zoneName);
            }
            else
            {
                newName += char.ToLower(zoneName);
            }

            return new Location(newId, newName);
        }

        #endregion

        #region Equality

        public bool Equals(Location other) =>
            Id == other.Id &&
            Name == other.Name;

        #endregion

        #region IResource
        
        long IResource.Id => Id;

        ResourceType IResource.ResourceType => ResourceTypes.Location;

        #endregion
    }
}