using System;

using Carbon.Data;

namespace Carbon.Platform.Web
{
    public class WebDb
    {
        public WebDb(IDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            WebComponents = new Dataset<WebComponent, long>(context);
            WebLibraries  = new Dataset<WebLibrary, long>(context);
        }

        public IDbContext Context { get; }

        public Dataset<WebComponent , long> WebComponents  { get; }
        public Dataset<WebLibrary   , long> WebLibraries   { get; }
    }
}