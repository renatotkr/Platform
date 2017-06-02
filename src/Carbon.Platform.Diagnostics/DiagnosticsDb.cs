using System;

using Carbon.Data;
using Carbon.Data.Sequences;

namespace Carbon.Platform.Diagnostics
{
    public class DiagnosticsDb
    {
        public DiagnosticsDb(IDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            context.Types.TryAdd(new BigIdHandler());

            BrowserExceptions = new Dataset<BrowserException, BigId>(context);
            Exceptions        = new Dataset<EnvironmentException, BigId>(context);
            Issues            = new Dataset<Issue, long>(context);
        }

        public IDbContext Context { get; }

        public Dataset<BrowserException, BigId>     BrowserExceptions { get; }
        public Dataset<EnvironmentException, BigId> Exceptions { get; }
        public Dataset<Issue, long>                 Issues { get; }

    }
}