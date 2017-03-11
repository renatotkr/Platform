using System;

namespace Carbon.Platform.Frontends
{
    using Data;
    using Versioning;

    public class FrontendDb
    {
        private readonly IDbContext context;

        public FrontendDb(IDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            
            Frontends        = new Dataset<Frontend, long>(context);
            FrontendBranches = new Dataset<FrontendBranch, (long, string)>(context);   
            FrontendReleases = new Dataset<FrontendRelease, (long, SemanticVersion)>(context); 
        }

        public IDbContext Context => context;

        public Dataset<Frontend, long>                           Frontends        { get; }
        public Dataset<FrontendBranch, (long, string)>           FrontendBranches { get; }
        public Dataset<FrontendRelease, (long, SemanticVersion)> FrontendReleases { get; }
    }

}