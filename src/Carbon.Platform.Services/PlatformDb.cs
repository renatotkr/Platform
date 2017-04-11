using System;

namespace Carbon.Platform
{
    using Apps;
    using CI;
    using Computing;
    using Data;
    using Hosting;
    using Logs;
    using Networking;
    using Security;
    using Storage;
    using Versioning;

    public class PlatformDb
    {
        public PlatformDb(IDbContext context)
        {
            // Ensure the db type handlers are registered
            context.Types.TryAdd(new SemanticVersionHandler());
            context.Types.TryAdd(new IPAddressHandler());
            context.Types.TryAdd(new IPAddressArrayHandler());
            context.Types.TryAdd(new HashHandler());
            context.Types.TryAdd(new JsonObjectHandler());
            context.Types.TryAdd(new StringArrayHandler());
            context.Types.TryAdd(new MacAddressHandler());
            context.Types.TryAdd(new Int64ArrayHandler());

            Context = context ?? throw new ArgumentNullException(nameof(context));

            // Environment -----------------------------------------------------------
            Environments          = new Dataset<AppEnvironment, long>(context);
            EnvironmentLocations  = new Dataset<EnvironmentLocation, (long, long)>(context);
            Locations             = new Dataset<LocationInfo, long>(context);

            // Apps ------------------------------------------------------------------
            Apps                  = new Dataset<AppInfo, long>(context);
            AppReleases           = new Dataset<AppRelease, (long, SemanticVersion)>(context);

            // Consent & Privacy-------------------------------------------------------
            Certificates          = new Dataset<CertificateInfo, long>(context);
            EncryptionKeys        = new Dataset<EncryptionKeyInfo, long>(context);

            // Computing & Storage ---------------------------------------------------
            Hosts                 = new Dataset<HostInfo,           long>(context);
            HostGroups            = new Dataset<HostGroup,          long>(context);
            HostTemplates         = new Dataset<HostTemplate,       long>(context);
            MachineImages         = new Dataset<MachineImageInfo,   long>(context);
            MachineTypes          = new Dataset<MachineType,        long>(context);
            Volumes               = new Dataset<VolumeInfo,         long>(context);

            // Networks --------------------------------------------------------------
            Networks              = new Dataset<NetworkInfo, long>(context);
            NetworkAddresses      = new Dataset<NetworkAddress, long>(context);
            NetworkInterfaces     = new Dataset<NetworkInterfaceInfo, long>(context);
            NetworkSecurityGroups = new Dataset<NetworkSecurityGroup, long>(context);
            NetworkPolicyRules    = new Dataset<NetworkSecurityGroupRule, long>(context);
            LoadBalancers         = new Dataset<LoadBalancer, long>(context);
            LoadBalancerListeners = new Dataset<LoadBalancerListener, long>(context);
            Subnets               = new Dataset<SubnetInfo, long>(context);

            // CI
            Deployments           = new Dataset<Deployment, long>(context);
            DeploymentTargets     = new Dataset<DeploymentTarget, (long, long)>(context);

            // Logging ---------------------------------------------------------------
            Activities = new Dataset<Activity, long>(context);

        }

        public IDbContext Context { get; }

        // Environment ------------------------------------------------
        public Dataset<AppEnvironment, long>                Environments         { get; }
        public Dataset<EnvironmentLocation, (long, long)>   EnvironmentLocations { get; }
        public Dataset<EnvironmentResource, long>           EnvironmentResources { get; }
        public Dataset<LocationInfo, long>                  Locations            { get; }

        // Apps  -----------------------------------------------------------------
        public Dataset<AppInfo, long>                       Apps            { get; }
        public Dataset<AppRelease, (long, SemanticVersion)> AppReleases     { get; }

        // Computing & Storage ---------------------------------------------------                          
        public Dataset<EncryptionKeyInfo,    long> EncryptionKeys        { get; }

        public Dataset<HealthCheck,          long> HealthChecks          { get; }
        public Dataset<HostInfo,             long> Hosts                 { get; }
        public Dataset<HostGroup,            long> HostGroups             { get; }
        public Dataset<HostTemplate,         long> HostTemplates         { get; }
        public Dataset<MachineImageInfo,     long> MachineImages         { get; }
        public Dataset<MachineType,          long> MachineTypes          { get; }
        public Dataset<VolumeInfo,           long> Volumes               { get; }

        // Networks --------------------------------------------------------------
        public Dataset<NetworkInfo,              long> Networks              { get; }
        public Dataset<NetworkAddress,           long> NetworkAddresses      { get; }
        public Dataset<NetworkInterfaceInfo,     long> NetworkInterfaces     { get; }
        public Dataset<NetworkSecurityGroup,     long> NetworkSecurityGroups       { get; }
        public Dataset<NetworkSecurityGroupRule, long> NetworkPolicyRules    { get; }
        public Dataset<LoadBalancer,             long> LoadBalancers        { get; }
        public Dataset<LoadBalancerListener,     long> LoadBalancerListeners { get; }
        public Dataset<SubnetInfo,               long> Subnets               { get; }

        // Hosting --------------------------------------------------------------
        public Dataset<CertificateInfo,          long> Certificates          { get; }

        // CI
        public Dataset<Deployment, long>               Deployments { get; }
        public Dataset<DeploymentTarget, (long, long)> DeploymentTargets { get; }

        // Logs -----------------------------------------------------------------
        public Dataset<Activity, long>  Activities { get; }
    }
}