using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Resources
{
    [DataContract]
    public struct ResourceType : IEquatable<ResourceType>
    {
        public ResourceType(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [DataMember(Name = "name", Order = 1)]
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
        public static readonly ResourceType Account              = "account";
        public static readonly ResourceType Entity               = "entity";   // Organization, Person, or IA
        public static readonly ResourceType User                 = "user";      
        public static readonly ResourceType Session              = "session";

        // Environments-------------------------------------------------------------------------------------

        public static readonly ResourceType Environment          = "environment"; // An environment actions take place within
        public static readonly ResourceType Location             = "location";

        // Computing  -----------------------------------------------------------------------------------------
        public static readonly ResourceType Cluster              = "cluster";
        public static readonly ResourceType Host                 = "host";          //  metal, vms, and containers
        public static readonly ResourceType HostTemplate         = "host:template";
        public static readonly ResourceType Image                = "host:image";
        public static readonly ResourceType MachineType          = "machine`type";
        public static readonly ResourceType Program              = "program";
        public static readonly ResourceType ProgramRelease       = "program:release";
        public static readonly ResourceType LoadBalancer         = "load`balancer";
        public static readonly ResourceType LoadBalancerListener = "load`balancer:listener";
        public static readonly ResourceType LoadBalancerRule     = "load`balancer:rule";

        // Storage --------------------------------------------------------------------------------------------
        public static readonly ResourceType Bucket               = "bucket";        
        public static readonly ResourceType Channel              = "channel";  // AKA stream / topic          
        public static readonly ResourceType Queue                = "queue";         
        public static readonly ResourceType Repository           = "repository";
        public static readonly ResourceType RepositoryBranch     = "repository:branch";
        public static readonly ResourceType RepositoryCommit     = "repository:commit"; 
        public static readonly ResourceType RepositoryTag        = "repository:tag";
        public static readonly ResourceType Volume               = "volume";
        public static readonly ResourceType VolumeSnapshot       = "volume:snapshot";

        // Rds --------------------------------------------------------------------
        public static readonly ResourceType Database             = "database";
        public static readonly ResourceType DatabaseCluster      = "database:cluster";
        public static readonly ResourceType DatabaseInstance     = "database:instance";
        public static readonly ResourceType DatabaseSchema       = "database:schema";
        public static readonly ResourceType DatabaseGrant        = "database:grant";
        public static readonly ResourceType DatabaseUser         = "database:user";

        // Vaults / KMS--------------------------------------------------------------------------------------------------
        public static readonly ResourceType Vault                = "vault";
        public static readonly ResourceType VaultGrant           = "vault:grant";
        public static readonly ResourceType VaultKey             = "vault:key";
        public static readonly ResourceType VaultSecret          = "vault:secret";

        public static readonly ResourceType Certificate          = "certificate";

        // Networking --------------------------------------------------------------------------------------------

        public static readonly ResourceType Network              = "network";
        public static readonly ResourceType NetworkAddress       = "network:address";
        public static readonly ResourceType NetworkInterface     = "network:nterface";
        public static readonly ResourceType NetworkPeer          = "network:peer";
        public static readonly ResourceType NetworkRouter        = "network:router";
        public static readonly ResourceType NetworkRoute         = "network:route";
        public static readonly ResourceType NetworkSecurityGroup = "network:security`group";
        public static readonly ResourceType Subnet               = "subnet";

        // Web -----------------------------------------------------------------------------------------------

        public static readonly ResourceType Domain               = "domain";
        public static readonly ResourceType DomainRecord         = "domain:record";
        public static readonly ResourceType DomainRegistration   = "domain:registration";
                                                                 
        public static readonly ResourceType Website              = "web:site";
        public static readonly ResourceType WebComponent         = "web:component";
        public static readonly ResourceType WebLibrary           = "web:library";
                                                                 
        public static readonly ResourceType DnsZone              = "dns:zone";
        public static readonly ResourceType DnsRecord            = "dns:record";

        // Health --------------------------------------------------------------------------------------------
        public static readonly ResourceType HealthCheck          = "health`check";

        // CI ------------------------------------------------------------------------------------------------
        public static readonly ResourceType Build                = "ci:build";
        public static readonly ResourceType Project              = "ci:project";
        public static readonly ResourceType Deployment           = "deployment";

        // Metrics --------------------------------------------------------------------------------------------
        public static readonly ResourceType Metric               = "metric";
        public static readonly ResourceType MetricDimension      = "metric:dimension";
    }
}