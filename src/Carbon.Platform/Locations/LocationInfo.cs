using System;
using System.Runtime.Serialization;

namespace Carbon.Platform
{
    using Data.Annotations;

    // Regions (May have multiple avaiability zones)
    // Zones

    [Dataset("Locations")]
    [DataIndex(IndexFlags.Unique, new[] { "providerId", "name" })]
    public class LocationInfo : ILocation, IEquatable<LocationInfo>, ICloudResource
    {
        public LocationInfo() { }

        public LocationInfo(
            ResourceProvider provider,
            ushort regionNumber,
            byte zoneNumber,
            string name,
            LocationFlags flags = 0)
        {
            var id = new LocationId {
                ProviderId   = provider.Id,
                RegionNumber = regionNumber,
                ZoneNumber   = zoneNumber,
                Flags        = (byte)flags,
            };

            Id = id.Value;
            ProviderId = provider.Id;
            Name = name;
        }

        [Member("id"), Key]
        public long Id { get; set; }

        [Member("providerId")]
        public int ProviderId { get; set; }

        // e.g. us-east-1, us-east1a
        [Member("name")]
        public string Name { get; set; }

        [Member("status")]
        public LocationStatus Status { get; set; }

        [Member("created")] // when the region was launched
        public DateTime Created { get; set; }

        #region Flags

        [IgnoreDataMember]
        public LocationFlags Flags =>
            (LocationFlags)LocationId.Create(Id).Flags;

        [IgnoreDataMember]
        public bool IsMultiRegional => 
            (Flags & LocationFlags.MultiRegional) != 0;

        #endregion

        #region IResource

        string ICloudResource.ResourceId => Name;

        [IgnoreDataMember]
        public ResourceProvider Provider => ResourceProvider.Get(ProviderId);
        
        [IgnoreDataMember]
        public ResourceType Type
        {
            get
            {
                var id = LocationId.Create(Id);

                return (id.ZoneNumber != 0)
                    ? ResourceType.Zone
                    : ResourceType.Region;
            }
        }

        #endregion

        [IgnoreDataMember]
        public string RegionName
        {
            get
            {
                if (Type == ResourceType.Region)
                {
                    return Name;
                }

                return Name.Substring(0, Name.Length - 1);
            }
        }

        [IgnoreDataMember]
        public string ZoneName
        {
            get
            {
                if (Type != ResourceType.Zone)
                {
                    return null;
                }

                return Name.Substring(Name.Length - 1);
            }
        }

        public LocationInfo WithZone(char zoneName)
        {
            #region Preconditions
            
            if (ProviderId == ResourceProvider.Microsoft.Id)
            {
                throw new Exception("Azure does not have zones");
            }

            #endregion

            var id = LocationId.Create(Id);

            id.ZoneNumber = LocationHelper.GetZoneNumber(zoneName);

            // AMAZON: us-east-1a
            // GOOGLE: us-central1-b, us-central1-c

            var name = Name;

            if (Provider == ResourceProvider.Google)
            {
                name += "-" + char.ToLower(zoneName);
            }
            else
            {
                name += char.ToLower(zoneName);
            }

            return new LocationInfo {
                Id         = id.Value,
                Name       = name,
                ProviderId = id.ProviderId
            };
        }

        public bool Equals(LocationInfo other) =>
            other.Id == Id &&
            other.ProviderId == ProviderId &&
            other.Name == Name;

        public static LocationInfo FromId(long id, string name) =>  
            new LocationInfo {
                Id         = id,
                ProviderId = LocationId.Create(id).ProviderId,
                Name       = name
            };
    }
}