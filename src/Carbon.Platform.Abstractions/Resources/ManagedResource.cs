using System;
using System.Text;

namespace Carbon.Platform.Resources
{
    public struct ManagedResource : IEquatable<ManagedResource>
    {
        public ManagedResource(ResourceProvider provider, ResourceType type, string id)
            : this(Platform.LocationId.Create(provider, 0, 0, 0), type, id) { }

        public ManagedResource(ILocation location, ResourceType type, string id)
            : this(Platform.LocationId.Create(location.Id), type, id) { }

        public ManagedResource(LocationId locationId, ResourceType type, string id)
        {
            LocationId = locationId;
            ProviderId = locationId.ProviderId;
            Type = type;
            ResourceId = id ?? throw new ArgumentNullException(nameof(id));
        }

        public int ProviderId { get; }

        public long LocationId { get; }

        public ResourceType Type { get; }

        public string ResourceId { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            var lid = Platform.LocationId.Create(LocationId);

            var provider = ResourceProvider.FromLocationId(LocationId);

            sb.Append(provider.Code);
            sb.Append(':');

            if (lid.RegionNumber != 0)
            {
                var location = Locations.Get(lid);

                sb.Append(location.Name);

                sb.Append(':');
            }

            sb.Append(Type.GetName());
            sb.Append('/');
            sb.Append(ResourceId);

            return sb.ToString();
        }

        #region Helpers

        public static ManagedResource EncryptionKey(ILocation location, string id) =>
            new ManagedResource(location, ResourceType.EncryptionKey, id);

        public static ManagedResource LoadBalancer(ILocation location, string id) =>
            new ManagedResource(location, ResourceType.LoadBalancer, id);

        public static ManagedResource Host(ILocation location, string id)
            => new ManagedResource(location, ResourceType.Host, id);

        public static ManagedResource HostGroup(ILocation location, string id)
            => new ManagedResource(location, ResourceType.HostGroup, id);

        public static ManagedResource Network(ILocation location, string id)
            => new ManagedResource(location, ResourceType.Network, id);
 
        public static ManagedResource NetworkInterface(ILocation location, string id)
            => new ManagedResource(location, ResourceType.NetworkInterface, id);

        public static ManagedResource Volume(ILocation location, string id)
            => new ManagedResource(location, ResourceType.Volume, id);

        
        // Repository
        // ...

        #endregion

        private static readonly char[] splitOn = { ':', '/' };

        // e.g. 
        // amzn:instance/i-453-352-18

        // amzn:us-east-1:bucket/carbon

        public static ManagedResource Parse(string text)
        {
            var parts = text.Split(splitOn);

            var provider = ResourceProvider.Parse(parts[0]);

            string regionName = null;
            string typeName = null;
            string id = null;

            if (parts.Length == 3)
            {
                typeName = parts[1];
                id = parts[2];
            }
            else if (parts.Length == 4)
            {
                regionName = parts[1];
                typeName = parts[2];
                id = parts[3];
            }

            var type = ResourceTypeHelper.Parse(typeName);

            if (regionName != null)
            {
                var location = Locations.Get(provider, regionName);

                return new ManagedResource(Platform.LocationId.Create(location.Id), type, id);
            }

            return new ManagedResource(provider, type, id);            
        }

        #region IEquatable

        bool IEquatable<ManagedResource>.Equals(ManagedResource other) => 
            LocationId == other.LocationId
            && Type == other.Type 
            && ResourceId == other.ResourceId;

        #endregion
    }

}

// 1/i-07e6001e0415497e4
// amzn:region/us-east-1

/*
aws:instance/i-453-352-18
aws:volume/vol-1a2b3c4d
aws:image/ami-1a2b3c4d
aws:bucket:us-east-1/name
google:instance/1234
google:bucket/name
*/
