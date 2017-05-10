using System;

using Carbon.Data;
using Carbon.Data.Sequences;
using Carbon.Versioning;

namespace Carbon.Platform.Web
{
    public class WebDb
    {
        public WebDb(IDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            
            Websites           = new Dataset<WebsiteInfo, long>(context, GetSequence("websites"));
            WebsiteReleases    = new Dataset<WebsiteRelease, (long, SemanticVersion)>(context);
            WebComponents      = new Dataset<WebComponent, long>(context, GetSequence("webComponents"));
            WebLibraries       = new Dataset<WebLibrary, long>(context, GetSequence("webLibraries"));
        }

        public IDbContext Context { get; }

        public DbSequence GetSequence(string name) => new DbSequence(name, Context);

        public Dataset<WebsiteInfo, long>                       Websites           { get; }
        public Dataset<WebsiteRelease, (long, SemanticVersion)> WebsiteReleases    { get; }
        public Dataset<WebComponent, long>                      WebComponents      { get; }
        public Dataset<WebLibrary, long>                        WebLibraries       { get; }
    }
}