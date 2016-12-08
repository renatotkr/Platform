using System.Data;

namespace Carbon.Platform
{
    using Apps;
    using Computing;
    using Frontends;
    using Data;
    using Networking;
    using Storage;

    public class PlatformDb
    {
        private readonly IDbContext context;

        public PlatformDb(IDbContext context)
        {
            this.context = context;
            
            Apps              = new Dataset<App>(context);
            AppInstances      = new Dataset<AppInstance>(context);
            AppReleases       = new Dataset<AppRelease>(context);
            AppEvents         = new Dataset<AppEvent>(context);

            // Frontends
            Frontends         = new Dataset<Frontend>(context);
            FrontendBranches  = new Dataset<FrontendBranch>(context);
            FrontendReleases  = new Dataset<FrontendRelease>(context);

            // Networks
            Networks          = new Dataset<Network>(context);
            NetworkInterfaces = new Dataset<NetworkInterfaceInfo>(context);

            Hosts             = new Dataset<Host>(context);
            Volumes           = new Dataset<VolumeInfo>(context);
            Images            = new Dataset<Image>(context);
        }

        public IDbConnection GetConnection()
            => context.GetConnection();

        // Apps
        public Dataset<App>                   Apps              { get; }
        public Dataset<AppEvent>              AppEvents         { get; }
        public Dataset<AppInstance>           AppInstances      { get; }
        public Dataset<AppRelease>            AppReleases       { get; }
        // AppErrors ?

        // Frontends
        public Dataset<Frontend>              Frontends         { get; }
        public Dataset<FrontendBranch>        FrontendBranches  { get; }
        public Dataset<FrontendRelease>       FrontendReleases  { get; }

        public Dataset<Image>                 Images            { get; }

        public Dataset<Network>               Networks          { get; }
        public Dataset<NetworkInterfaceInfo>  NetworkInterfaces { get; } 
  
        public Dataset<Host>                  Hosts             { get; }

        public Dataset<VolumeInfo>            Volumes           { get; }
    }
}