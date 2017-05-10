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

            EnvironmentExceptions = new Dataset<EnvironmentException, BigId>(context);
        }

        public IDbContext Context { get; }

        public Dataset<BrowserException, BigId> BrowserExceptions { get; }

        public Dataset<EnvironmentException, BigId> EnvironmentExceptions { get; }

    }
}