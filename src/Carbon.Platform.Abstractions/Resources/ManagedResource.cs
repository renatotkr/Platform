using System;
using System.Text;

namespace Carbon.Platform.Resources
{
    public struct ManagedResource : IEquatable<ManagedResource>
    {
        public ManagedResource(ResourceProvider provider, ResourceType type, string id)
            : this(provider, Locations.Global, type, id) { }

        public ManagedResource(ResourceProvider provider, ILocation location, ResourceType type, string id)
        {
            #region Preconditions

            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Must not be null or empty", nameof(id));

            #endregion

            ProviderId = provider.Id;
            LocationId = location.Id;
            Type       = type;
            ResourceId = id;
        }

        public int ProviderId { get; }

        public int LocationId { get; }

        public ResourceType Type { get; }

        public string ResourceId { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            var provider = ResourceProvider.Get(ProviderId);

            sb.Append(provider.Code);
            sb.Append(':');

            if (LocationId != 0)
            {
                var location = Locations.Get(LocationId);

                sb.Append(location.Name);

                sb.Append(':');
            }

            sb.Append(Type.GetName());
            sb.Append('/');
            sb.Append(ResourceId);

            return sb.ToString();
        }

        #region Helpers

        private static ManagedResource FromLocation(ILocation location, ResourceType type, string id)
        {
            var provider = ResourceProvider.Get(Platform.LocationId.Create(location.Id).ProviderId);

            return new ManagedResource(provider, location, type, id);
        }

        public static ManagedResource Bucket(ILocation location, string id) =>
            FromLocation(location, ResourceType.Bucket, id);

        public static ManagedResource DatabaseCluster(ILocation location, string id) => 
            FromLocation(location, ResourceType.DatabaseCluster, id);

        public static ManagedResource DatabaseInstance(ILocation location, string id) => 
            FromLocation(location, ResourceType.DatabaseInstance, id);

        public static ManagedResource EncryptionKey(ILocation location, string id) =>
            FromLocation(location, ResourceType.EncryptionKey, id);

        public static ManagedResource LoadBalancer(ILocation location, string id) =>
            FromLocation(location, ResourceType.LoadBalancer, id);

        public static ManagedResource Host(ILocation location, string id) => 
            FromLocation(location, ResourceType.Host, id);

        public static ManagedResource HostGroup(ILocation location, string id) => 
            FromLocation(location, ResourceType.HostGroup, id);

        public static ManagedResource HostTemplate(ResourceProvider provider, string id) =>
            new ManagedResource(provider, ResourceType.HostTemplate, id);

        public static ManagedResource MachineImage(ILocation location, string id) =>
           FromLocation(location, ResourceType.MachineImage, id);

        public static ManagedResource Network(ILocation location, string id) => 
            FromLocation(location, ResourceType.Network, id);
 
        public static ManagedResource NetworkInterface(ILocation location, string id) => 
            FromLocation(location, ResourceType.NetworkInterface, id);

        public static ManagedResource Subnet(ILocation location, string id) => 
            FromLocation(location, ResourceType.Subnet, id);

        public static ManagedResource Volume(ILocation location, string id) => 
            FromLocation(location, ResourceType.Volume, id);

        public static ManagedResource Repository(ResourceProvider provider, string accountName, string repositoryName) =>
            new ManagedResource(provider, ResourceType.Repository, $"{accountName}/{repositoryName}");

        public static ManagedResource Queue(ILocation location, string id) =>
            FromLocation(location, ResourceType.Queue, id);

        public static ManagedResource Channel(ILocation location, string id) =>
            FromLocation(location, ResourceType.Channel, id);

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

                return FromLocation(location, type, id);
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

/*
aws:host/i-453-352-18
aws:location/us-east-1
aws:volume/vol-1a2b3c4d
aws:image/ami-1a2b3c4d
aws:bucket:us-east-1/name
gcp:host/1234
gcp:bucket/name
*/
