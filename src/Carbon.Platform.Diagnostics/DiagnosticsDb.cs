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

            context.Types.TryAdd(new UidHandler());

            Issues     = new Dataset<Issue, long>(context);
        }

        public IDbContext Context { get; }

        // Traces?

        public Dataset<Issue, long>        Issues { get; }
    }
}