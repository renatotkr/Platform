using System;
using System.Runtime.Serialization;

namespace Carbon.Platform
{
    using Data.Annotations;

    // Locations may be regions or zones

    [Dataset("Locations")]
    [DataIndex(IndexFlags.Unique, new[] { "providerId", "name" })]
    public class LocationInfo : ILocation, IEquatable<LocationInfo>
    {
        public LocationInfo() { }

        public LocationInfo(long id, string name, LocationStatus status = LocationStatus.Healthy)
        {
            Id          = id;
            ProviderId  = LocationId.Create(id).ProviderId;
            Name        = name ?? throw new ArgumentNullException(nameof(name));
            Status      = status;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("providerId")]
        public int ProviderId { get; }

        // e.g. us-east-1
        // us-east1a
        [Member("name")]
        [StringLength(63)]
        public string Name { get; }

        [Member("status")]
        public LocationStatus Status { get; }

        //  // when the region was launched
        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        #region Flags

        [IgnoreDataMember]
        public LocationFlags Flags => (LocationFlags)LocationId.Create(Id).Flags;

        [IgnoreDataMember]
        public bool IsMultiRegional => 
            (Flags & LocationFlags.MultiRegional) != 0;

        #endregion

        #region IResource

        string IManagedResource.ResourceId => Name;

        [IgnoreDataMember]
        public ResourceProvider Provider => ResourceProvider.Get(ProviderId);
        
        [IgnoreDataMember]
        public ResourceType ResourceType
        {
            get
            {
                var id = LocationId.Create(Id);

                return (id.ZoneNumber != 0)
                    ? ResourceType.Zone
                    : ResourceType.Region;
            }
        }

        long IManagedResource.LocationId => Id;


        #endregion

        public bool Equals(LocationInfo other) =>
            other.Id == Id &&
            other.Name == Name;
    }
}