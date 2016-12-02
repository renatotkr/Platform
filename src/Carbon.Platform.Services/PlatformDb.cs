namespace Carbon.Platform
{
    using Backends;
    using Computing;
    using Frontends;
    using Data;
    using Networking;
    using Storage;

    public class PlatformDb
    {
        public PlatformDb(IDbContext context)
        {
            Backends          = new Dataset<Backend>(context);
            BackendInstances  = new Dataset<BackendInstance>(context);
            Frontends         = new Dataset<Frontend>(context);
            FrontendReleases  = new Dataset<FrontendRelease>(context);
            Networks          = new Dataset<Network>(context);
            NetworkInterfaces = new Dataset<NetworkInterfaceInfo>(context);
            Programs          = new Dataset<Program>(context);
            ProgramReleases   = new Dataset<ProgramRelease>(context);
            Hosts             = new Dataset<Host>(context);
            Volumes           = new Dataset<VolumeInfo>(context);
            Images            = new Dataset<Image>(context);
        }

        public Dataset<Backend>               Backends          { get; }
        public Dataset<BackendInstance>       BackendInstances  { get; }

        public Dataset<Frontend>              Frontends         { get; }
        public Dataset<FrontendRelease>       FrontendReleases  { get; }
        public Dataset<Image>                 Images            { get; }

        public Dataset<Network>               Networks          { get; }
        public Dataset<NetworkInterfaceInfo>  NetworkInterfaces { get; } 

        public Dataset<Program>               Programs          { get; }
        public Dataset<ProgramRelease>        ProgramReleases   { get; }

        public Dataset<Host>                  Hosts             { get; }

        public Dataset<VolumeInfo>            Volumes           { get; }
    }
}

// Rebase on Dynamo?
