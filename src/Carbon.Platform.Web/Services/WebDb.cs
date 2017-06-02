using System;

using Carbon.Data;

namespace Carbon.Platform.Web
{
    public class WebDb
    {
        public WebDb(IDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            
            Websites        = new Dataset<WebsiteInfo, long>(context);
            WebsiteReleases = new Dataset<WebsiteRelease, long>(context);
            WebComponents   = new Dataset<WebComponent, long>(context);
            WebLibraries    = new Dataset<WebLibrary, long>(context);
        }

        public IDbContext Context { get; }

        public Dataset<WebsiteInfo,    long> Websites           { get; }
        public Dataset<WebsiteRelease, long> WebsiteReleases    { get; }
        public Dataset<WebComponent,   long> WebComponents      { get; }
        public Dataset<WebLibrary,     long> WebLibraries       { get; }
    }
}