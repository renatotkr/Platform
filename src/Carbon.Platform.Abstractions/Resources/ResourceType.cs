﻿using System;

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
        public static ResourceType Account       = "account";
        public static ResourceType Entity        = "entity";   // Organization, Person, or IA
        public static ResourceType User          = "user";      
        public static ResourceType Session       = "user:session";

        // Environments-------------------------------------------------------------------------------------

        public static ResourceType Environment   = "environment"; // An environment actions take place within
        public static ResourceType Location      = "location";

        // Computing  -----------------------------------------------------------------------------------------
        public static ResourceType Cluster              = "cluster";
        public static ResourceType Host                 = "host";          //  metal, vms, and containers
        public static ResourceType HostTemplate         = "host:template";
        public static ResourceType Image                = "host:image";
        public static ResourceType MachineType          = "machineType";
        public static ResourceType Program              = "program";
        public static ResourceType ProgramRelease       = "program:release";
        public static ResourceType LoadBalancer         = "loadBalancer";
        public static ResourceType LoadBalancerListener = "loadBalancer:listener";
        public static ResourceType LoadBalancerRule     = "loadBalancer:rule";

        // Storage --------------------------------------------------------------------------------------------
        public static ResourceType Bucket              = "bucket";        
        public static ResourceType Channel             = "channel";               // AKA Stream / Topic           
        public static ResourceType Queue               = "queue";         
        public static ResourceType Repository          = "repository";
        public static ResourceType RepositoryBranch    = "repository:branch";
        public static ResourceType RepositoryCommit    = "repository:commit"; 
        public static ResourceType RepositoryTag       = "repository:tag";
        public static ResourceType Volume              = "volume";
        public static ResourceType VolumeSnapshot      = "volume:snapshot";

        // Rds --------------------------------------------------------------------
        public static ResourceType Database         = "database";
        public static ResourceType DatabaseCluster  = "database:cluster";
        public static ResourceType DatabaseInstance = "database:instance";
        public static ResourceType DatabaseSchema   = "database:schema";
        public static ResourceType DatabaseGrant    = "database:grant";
        public static ResourceType DatabaseUser     = "database:user";

        // Vaults --------------------------------------------------------------------------------------------------
        public static ResourceType Vault       = "vault";
        public static ResourceType VaultGrant  = "vault:grant";
        public static ResourceType VaultKey    = "vault:key";
        public static ResourceType VaultSecret = "vault:secret";

        // Networking --------------------------------------------------------------------------------------------

        public static ResourceType Network              = "network";
        public static ResourceType NetworkAddress       = "network:address";
        public static ResourceType NetworkInterface     = "network:nterface";
        public static ResourceType NetworkPeer          = "network:peer";
        public static ResourceType NetworkRouter        = "network:router";
        public static ResourceType NetworkRoute         = "network:route";
        public static ResourceType NetworkSecurityGroup = "network:securityGroup";
        public static ResourceType Subnet               = "subnet";

        
        // Web -----------------------------------------------------------------------------------------------

        public static ResourceType Domain        = "domain";
        public static ResourceType Certificate   = "certificate"; // web: ?
                            
        public static ResourceType Website       = "web:site";
        public static ResourceType WebComponent  = "web:component";
        public static ResourceType WebLibrary    = "web:library";
        
        public static ResourceType DnsZone       = "dns:zone";
        public static ResourceType DnsRecord     = "dns:record";

        // Health --------------------------------------------------------------------------------------------
        public static ResourceType HealthCheck  = "health:check";

        // CI ------------------------------------------------------------------------------------------------
        public static ResourceType Build         = "ci:build";
        public static ResourceType Project       = "ci:project";
        public static ResourceType Deployment    = "ci:deployment";

        // Metrics --------------------------------------------------------------------------------------------
        public static ResourceType Metric          = "metric";
        public static ResourceType MetricDimension = "metric:dimension";

    }
}