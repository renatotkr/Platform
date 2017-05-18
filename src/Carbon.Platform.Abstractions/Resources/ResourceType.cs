using System;

namespace Carbon.Platform.Resources
{
    public struct ResourceType : IEquatable<ResourceType>
    {
        public ResourceType(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public override string ToString() => Name;

        public static implicit operator ResourceType(string name)
        {
            return new ResourceType(name);
        }

        #region IEquality

        public bool Equals(ResourceType other)
        {
            return this.Name == other.Name;
        }

        #endregion
    }

    public static class ResourceTypes
    {
        // IAM --------------------------------------------------------------------------------------------
        public static ResourceType Account = "account";
        public static ResourceType Entity  = "entity";   // Organization, Person, or IA
        public static ResourceType User    = "user";      
        public static ResourceType Session = "session";

        // Environments-------------------------------------------------------------------------------------

        public static ResourceType Environment   = "environment"; // An environment actions take place within
        public static ResourceType Location      = "location";

        // Computing  -----------------------------------------------------------------------------------------
        public static ResourceType Host                 = "host";            // includes metal, vm instances, and container instances
        public static ResourceType HostGroup            = "hostGroup";       // A group of hosts with a specific configuration
        public static ResourceType HostTemplate         = "hostTemplate";
        public static ResourceType MachineImage         = "machineImage";
        public static ResourceType MachineType          = "machineType";
        public static ResourceType Program              = "program";

        public static ResourceType LoadBalancer         = "loadBalancer";
        public static ResourceType LoadBalancerListener = "loadBalancerListener";
        public static ResourceType LoadBalancerRule     = "loadBalancerRule";

        // Storage --------------------------------------------------------------------------------------------
        public static ResourceType Bucket              = "bucket";        
        public static ResourceType Channel             = "channel";               // AKA Stream / Topic           
        public static ResourceType Database            = "database";
        public static ResourceType DatabaseCluster     = "databaseCluster";
        public static ResourceType DatabaseInstance    = "databaseInstance";
        public static ResourceType DatabaseSchema      = "databaseSchema";
        public static ResourceType Queue               = "queue";         
        public static ResourceType Repository          = "repository";
        public static ResourceType RepositoryCommit    = "commit";             
        public static ResourceType Volume              = "volume";                // AKA drive

        // KMS --------------------------------------------------------------------------------------------------
        public static ResourceType DataEncryptionKey   = "dek";           
        public static ResourceType EncryptionKey       = "encryptionKey";         // master encryption keys

        // Networking --------------------------------------------------------------------------------------------

        public static ResourceType Network              = "network";
        public static ResourceType Subnet               = "subnet";
        public static ResourceType NetworkAddress       = "networkAddress";
        public static ResourceType NetworkInterface     = "networkInterface";
        public static ResourceType NetworkPeer          = "networkPeer";
        public static ResourceType NetworkRouter        = "networkRouter";
        public static ResourceType NetworkRoute         = "networkRoute";

        // - Security ---
        public static ResourceType NetworkSecurityGroup = "networkSecurityGroup";

        // - DNS -           
        public static ResourceType DnsZone            = "dnsZone";
        public static ResourceType DnsRecord          = "dnsRecord";

        // Hosting --------------------------------------------------------------------------------------------

        public static ResourceType Domain        = "domain";
        public static ResourceType Certificate   = "certificate";
                            
        public static ResourceType Website       = "website";
        public static ResourceType WebComponent  = "webComponent";
        public static ResourceType WebLibrary    = "webLibrary";

        public static ResourceType HealthCheck   = "healthCheck";

        // CI ------------------------------------------------------------------------------------------------
        public static ResourceType Build         = "build";
        public static ResourceType Deployment    = "deployment";

        // Metrics --------------------------------------------------------------------------------------------
        public static ResourceType Metric = "metric";
    }
}