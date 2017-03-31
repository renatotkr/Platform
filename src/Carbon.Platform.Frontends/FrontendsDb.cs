using System;

namespace Carbon.Platform.Frontends
{
    using Data;
    using Versioning;

    public class FrontendDb
    {
        public FrontendDb(IDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            
            Frontends        = new Dataset<Frontend, long>(context);
            FrontendReleases = new Dataset<FrontendRelease, (long, SemanticVersion)>(context); 
        }

        public IDbContext Context { get; }

        public Dataset<Frontend, long>                           Frontends        { get; }
        public Dataset<FrontendRelease, (long, SemanticVersion)> FrontendReleases { get; }
    }

}