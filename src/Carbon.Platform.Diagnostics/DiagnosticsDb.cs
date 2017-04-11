using System;

using Carbon.Data;

namespace Carbon.Platform.Diagnostics
{
    public class DiagnosticsDb
    {
        public DiagnosticsDb(IDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            context.Types.TryAdd(new BigIdHandler());

            Exceptions = new Dataset<ServerException, long>(context);
        }

        public IDbContext Context { get; }

        public Dataset<ServerException, long> Exceptions { get; }
    }
}