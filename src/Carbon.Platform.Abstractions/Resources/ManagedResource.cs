using System;
using System.Text;
using Carbon.Extensions;

namespace Carbon.Platform.Resources
{
    public struct ManagedResource : IEquatable<ManagedResource>
    {
        public ManagedResource(ResourceProvider provider, ResourceType type, string id)
            : this(provider, Locations.Global, type, id) { }

        public ManagedResource(
            ResourceProvider provider,
            ILocation location, 
            ResourceType type, 
            string id)
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

            sb.Append(Type.Name);
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
            FromLocation(location, ResourceTypes.Bucket, id);

        public static ManagedResource Build(ILocation location, string id) =>
            FromLocation(location, ResourceTypes.Build, id);

        public static ManagedResource Cluster(ResourceProvider provider, string id) =>
            new ManagedResource(provider, ResourceTypes.Cluster, id);

        public static ManagedResource DatabaseCluster(ILocation location, string id) => 
            FromLocation(location, ResourceTypes.DatabaseCluster, id);

        public static ManagedResource DatabaseInstance(ILocation location, string id) => 
            FromLocation(location, ResourceTypes.DatabaseInstance, id);

        public static ManagedResource EncryptionKey(ILocation location, string id) =>
            FromLocation(location, ResourceTypes.VaultGrant, id);

        public static ManagedResource Environment(ResourceProvider provider, string id) =>
            new ManagedResource(provider, ResourceTypes.Environment, id);

        public static ManagedResource LoadBalancer(ILocation location, string id) =>
            FromLocation(location, ResourceTypes.LoadBalancer, id);

        public static ManagedResource Host(ResourceProvider provider, string id) =>
            new ManagedResource(provider, ResourceTypes.Host, id);

        public static ManagedResource Host(ILocation location, string id) => 
            FromLocation(location, ResourceTypes.Host, id);

        public static ManagedResource HostTemplate(ResourceProvider provider, string id) =>
            new ManagedResource(provider, ResourceTypes.HostTemplate, id);

        public static ManagedResource Image(ILocation location, string id) =>
           FromLocation(location, ResourceTypes.Image, id);

        public static ManagedResource Volume(ILocation location, string id) => 
            FromLocation(location, ResourceTypes.Volume, id);

        public static ManagedResource Repository(ResourceProvider provider, string accountName, string repositoryName) =>
            new ManagedResource(provider, ResourceTypes.Repository, $"{accountName}/{repositoryName}");

        public static ManagedResource Queue(ILocation location, string id) =>
            FromLocation(location, ResourceTypes.Queue, id);

        public static ManagedResource Channel(ILocation location, string id) =>
            FromLocation(location, ResourceTypes.Channel, id);

        #endregion
        
        #region Networking

        public static ManagedResource Network(ILocation location, string id) =>
            FromLocation(location, ResourceTypes.Network, id);

        public static ManagedResource NetworkInterface(ILocation location, string id) =>
            FromLocation(location, ResourceTypes.NetworkInterface, id);

        public static ManagedResource NetworkSecurityGroup(ILocation location, string id) =>
            FromLocation(location, ResourceTypes.NetworkSecurityGroup, id);

        public static ManagedResource Subnet(ILocation location, string id) =>
            FromLocation(location, ResourceTypes.Subnet, id);

        #endregion

        // e.g. 

        /*
        arn:partition:service:region:account-id:resource
        arn:partition:service:region:account-id:resourcetype/resource
        arn:partition:service:region:account-id:resourcetype:resource
        */

        // aws:host/i-453-352-18
        // aws:us-east-1:bucket/carbon

        public static ManagedResource Parse(string text)
        {
            var segments = text.Split(Seperators.ForwardSlash); // '/'

            if (segments.Length != 2)
            {
                throw new Exception("missing id seperator '/')");
            }

            var parts = segments[0].Split(Seperators.Colon); // ':'

            var provider = ResourceProvider.Parse(parts[0]);

            string locationName = null;
            string typeName = null;

            if (parts.Length == 1)
            {
                typeName = "unknown";
            }
            if (parts.Length == 2)
            {
                typeName = parts[1];
            }
            else if (parts.Length == 3)
            {
                locationName = parts[1];
                typeName   = parts[2];
            }

            var id = segments[1];

            var resourceType = new ResourceType(typeName); // normalize?

            if (locationName != null)
            {
                var location = Locations.Get(provider, locationName);

                return FromLocation(location, resourceType, id);
            }

            return new ManagedResource(provider, resourceType, id);            
        }

        #region IEquatable

        bool IEquatable<ManagedResource>.Equals(ManagedResource other) => 
            LocationId == other.LocationId
            && Type.Name == other.Type.Name
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
