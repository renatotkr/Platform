using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform
{
    [Dataset("Locations")]
    [UniqueIndex("providerId", "name")]
    public class LocationInfo : ILocation, IEquatable<LocationInfo>
    {
        public LocationInfo() { }

        public LocationInfo(
            int id,
            string name, 
            LocationStatus status = LocationStatus.Healthy)
        {
            Id         = id;
            ProviderId = LocationId.Create(id).ProviderId;
            Name       = name ?? throw new ArgumentNullException(nameof(name));
            Status     = status;
        }

        [Member("id"), Key]
        public int Id { get; }

        [Member("providerId")]
        public int ProviderId { get; }

        [Member("name")]
        [StringLength(63)]
        public string Name { get; }
        
        [Member("status")]
        public LocationStatus Status { get; }

        public LocationType Type => LocationId.Create(Id).Type;

        #region Stats

        [Member("hostCount")]
        public int HostCount { get; }

        #endregion

        #region IResource

        [IgnoreDataMember]
        public ResourceProvider Provider => ResourceProvider.Get(ProviderId);
    
        #endregion

        #region Timestamps

        // when the region was launched
        [Member("created"), Timestamp]
        public DateTime Created { get; set; }

        #endregion

        #region Equality

        public bool Equals(LocationInfo other) =>
            other.Id == Id &&
            other.Name == Name;

        #endregion
    }
}