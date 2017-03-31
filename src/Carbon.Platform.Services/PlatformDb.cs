﻿using System;

namespace Carbon.Platform
{
    using Apps;
    using Certificates;
    using Computing;
    using Data;
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
            context.Types.TryAdd(new IPAddressListHandler());
            context.Types.TryAdd(new ListenerHandler());
            context.Types.TryAdd(new HashHandler());
            context.Types.TryAdd(new ListenerListHandler());
            context.Types.TryAdd(new HealthCheckHandler());
            context.Types.TryAdd(new JsonObjectHandler());
            context.Types.TryAdd(new StringArrayHandler());
            context.Types.TryAdd(new ThresholdHandler());

            Context = context ?? throw new ArgumentNullException(nameof(context));

            // Apps ------------------------------------------------------------------
            Apps = new Dataset<App, long>(context);
            AppInstances        = new Dataset<AppInstance, (long, long)>(context);
            AppReleases         = new Dataset<AppRelease, (long, SemanticVersion)>(context);
            AppEvents           = new Dataset<AppEvent, long>(context);
            AppErrors           = new Dataset<AppError, long>(context);

            Certificates        = new Dataset<CertificateInfo, long>(context);
            EncryptionKeys      = new Dataset<EncryptionKeyInfo, long>(context);


            // Backends --------------------------------------------------------------
            Backends            = new Dataset<BackendInfo, long>(context);
            Locations           = new Dataset<LocationInfo, long>(context);
            HealthChecks        = new Dataset<HealthCheck, long>(context);
            HostTemplates       = new Dataset<HostTemplate, long>(context);

            // Computing & Storage ---------------------------------------------------
            Hosts               = new Dataset<HostInfo, long>(context);
            Volumes             = new Dataset<VolumeInfo, long>(context);
            Images              = new Dataset<ImageInfo, long>(context);

            // Networks --------------------------------------------------------------
            Networks            = new Dataset<NetworkInfo, long>(context);
            NetworkAcls         = new Dataset<NetworkAcl, long>(context);
            NetworkAddresses    = new Dataset<NetworkAddress, long>(context);

            NetworkInterfaces  = new Dataset<NetworkInterfaceInfo, long>(context);
            NetworkProxies      = new Dataset<NetworkProxy, long>(context);
            NetworkRules        = new Dataset<NetworkAclRule, long>(context);
            Subnets             = new Dataset<SubnetInfo, long>(context);
        }

        public IDbContext Context { get; }

        // Apps  -----------------------------------------------------------------
        public Dataset<App, long>                           Apps { get; }
        public Dataset<AppEvent, long>                      AppEvents { get; }
        public Dataset<AppInstance, (long, long)>           AppInstances { get; }
        public Dataset<AppRelease, (long, SemanticVersion)> AppReleases { get; }
        public Dataset<AppError, long>                      AppErrors { get; }

        public Dataset<CertificateInfo, long>               Certificates   { get; }
        public Dataset<EncryptionKeyInfo, long>             EncryptionKeys { get; }

        // Backends --------------------------------------------------------------
        public Dataset<BackendInfo,  long> Backends  { get; }
        public Dataset<LocationInfo, long> Locations { get; }
        public Dataset<HealthCheck,  long> HealthChecks { get; }
        public Dataset<HostTemplate, long> HostTemplates { get; }

        // Computing & Storage ---------------------------------------------------                          
        public Dataset<HostInfo,   long> Hosts   { get; }
        public Dataset<ImageInfo,  long> Images  { get; }
        public Dataset<VolumeInfo, long> Volumes { get; }

        
        // Databases -------------------------------------------------------------
        /*
        public Dataset<DatabaseInfo,     long> Databases         { get; }
        public Dataset<DatabaseInstance, long> DatabaseInstances { get; } // DatabaseId, HostId?
        public Dataset<DatabaseInfo,     long> DatabaseBackups   { get; }
        public Dataset<DatabaseInfo,     long> DatabaseClusters  { get; }
        */

        // Networks --------------------------------------------------------------
        public Dataset<NetworkInfo,          long> Networks          { get; }
        public Dataset<NetworkAddress,       long> NetworkAddresses  { get; }
        public Dataset<NetworkAcl,           long> NetworkAcls       { get; }
        public Dataset<NetworkInterfaceInfo, long> NetworkInterfaces { get; }
        public Dataset<NetworkProxy,         long> NetworkProxies    { get; }
        public Dataset<NetworkAclRule,          long> NetworkRules      { get; }
        public Dataset<SubnetInfo,           long> Subnets           { get; }
    }
}